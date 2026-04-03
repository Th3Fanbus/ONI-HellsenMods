using Klei;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace TeleStorage
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TeleStorage : KMonoBehaviour
	{
		private static readonly StringBuilder builder = new(256);
		private static StatusItem? filterStatusItem;

		public ConduitType Type;

		/* TODO: Figure out a way to initialise this properly */
		[Serialize]
		public float Flow = 100000f;

		[MyCmpReq]
		public Filterable? filterable;

		private int inputCell = -1;
		private int outputCell = -1;

		public SimHashes FilteredElement { get; private set; } = SimHashes.Void;

		public static readonly EventSystem.IntraObjectHandler<TeleStorage> OnCopySettingsDelegate = new(static delegate (TeleStorage component, object data) {
			component.OnCopySettings(data);
		});

		public void OnCopySettings(object data)
		{
			TeleStorage component = ((GameObject)data).GetComponent<TeleStorage>();
			if (component != null) {
				Flow = component.Flow;
			}
		}

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			Subscribe((int)GameHashes.CopySettings, OnCopySettingsDelegate);
			InitialiseStatusItems();
		}

		private void InitialiseStatusItems()
		{
			filterStatusItem ??= new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID) {
				resolveStringCallback = (str, data) => {
					str = data is TeleStorage tele && tele.IsValidFilter
						? ElementLoader.FindElementByHash(tele.FilteredElement).name
						: STRINGS.BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED;
					return string.Format(STRINGS.BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, str);
				},
				conditionalOverlayCallback = new Func<HashedString, object, bool>((mode, data) => TeleStorageUtils.GetViewMode(Type) == mode)
			};
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			Building building = GetComponent<Building>();
			inputCell = building.GetUtilityInputCell();
			outputCell = building.GetUtilityOutputCell();

			TeleStorageUtils.GetFlowManager(Type).AddConduitUpdater(ConduitUpdate);

			if (filterable != null) {
				OnFilterChanged(filterable.SelectedTag);
				filterable.onFilterChanged += new Action<Tag>(OnFilterChanged);
			}
			GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, filterStatusItem, this);

			TeleStorageData.Instance?.storageContainers.Add(this);
		}

		public override void OnCleanUp()
		{
			TeleStorageUtils.GetFlowManager(Type).RemoveConduitUpdater(ConduitUpdate);
			TeleStorageData.Instance?.storageContainers.Remove(this);
			base.OnCleanUp();
		}

		private bool IsValidFilter => FilteredElement != SimHashes.Void && FilteredElement != SimHashes.Vacuum;

		private bool IsOperational => IsValidFilter && GetComponent<Operational>().IsOperational;

		public void FireRefresh()
		{
			try {
				Trigger((int)GameHashes.OnStorageChange);
			} catch (Exception ex) {
				Debug.LogWarning(ex.ToString());
			}
		}

		private void OnFilterChanged(Tag tag)
		{
			Element element = ElementLoader.GetElement(tag);
			if (element != null) {
				FilteredElement = element.id;
			}
			GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, !IsValidFilter, null);
		}

		private void ConduitUpdate(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(Type);
			if (flowManager == null) {
				return;
			}
			ConduitFlow.ConduitContents inputContents = flowManager.GetContents(inputCell);
			TeleStorageData.Instance?.AddOrUpdateStored(Type, inputContents.element, (element, inputStored) => {
				if (inputContents.mass > 0.0f && !float.IsNaN(inputStored.temperature) && !float.IsNaN(inputContents.temperature)) {
					inputStored.temperature = GameUtil.GetFinalTemperature(inputStored.temperature, inputStored.mass, inputContents.temperature, inputContents.mass);
					inputStored.mass += inputContents.mass;
					SimUtil.DiseaseInfo diseaseInfo = SimUtil.CalculateFinalDiseaseInfo(inputContents.diseaseIdx, inputContents.diseaseCount, inputStored.diseaseIdx, inputStored.diseaseCount);
					inputStored.diseaseIdx = diseaseInfo.idx;
					inputStored.diseaseCount = diseaseInfo.count;
					flowManager.RemoveElement(inputCell, inputContents.mass);
					TeleStorageData.Instance.FireRefresh();
				}
				return inputStored;
			});
			if (!IsOperational) {
				return;
			}
			TeleStorageData.Instance?.AddOrUpdateStored(Type, FilteredElement, (element, outputStored) => {
				float possibleOutput = Math.Min(outputStored.mass, Flow / TeleStorageFlowControl.GramsPerKilogram);
				if (possibleOutput > 0.0f) {
					float delta = flowManager.AddElement(outputCell, FilteredElement, possibleOutput, outputStored.temperature, 0, 0);
					outputStored.mass -= delta;
					TeleStorageData.Instance.FireRefresh();
				}
				return outputStored;
			});
		}

		public int AddStorageItems(CollapsibleDetailContentPanel targetPanel, int num = 0)
		{
			foreach (KeyValuePair<SimHashes, StoredItem> pair in TeleStorageData.Instance?.GetStoredElements(Type) ?? new()) {
				SimHashes element = pair.Key;
				StoredItem item = pair.Value;
				if (item.mass > 0.0f) {
					builder.Clear();
					Element elementObj = ElementLoader.FindElementByHash(element);
					builder.Append(elementObj.name);
					builder.Append(": ");
					builder.Append(GameUtil.GetFormattedMass(item.mass));
					builder.Append(" at ");
					builder.Append(GameUtil.GetFormattedTemperature(item.temperature));
					builder.Append(" state ");
					builder.Append(elementObj.state);
					string tooltip = "";
					if (item.diseaseIdx != byte.MaxValue) {
						builder.Append("\n • ");
						builder.Append(GameUtil.GetFormattedDisease(item.diseaseIdx, item.diseaseCount, false));
						tooltip = GameUtil.GetFormattedDisease(item.diseaseIdx, item.diseaseCount, true);
					}
					targetPanel.SetLabel("storage_" + num.ToString(), builder.ToString(), tooltip);
					++num;
				}
			}
			return num;
		}
	}
}
