using Klei;
using KSerialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace TeleStorage
{
	[SerializationConfig(MemberSerialization.OptIn)]
	public class TeleStorage : KMonoBehaviour, ISaveLoadable
	{
		private static StatusItem? filterStatusItem = null;

		[SerializeField]
		public ConduitType Type;

		[Serialize]
		public float Flow = 100000f;

		[Serialize]
		public Tag FilteredTag;

		private static readonly StringBuilder builder = new(128);

		private Filterable filterable = new();
		private int inputCell = -1;
		private int outputCell = -1;

		public SimHashes FilteredElement { get; private set; } = SimHashes.Void;

		public override void OnPrefabInit()
		{
			base.OnPrefabInit();
			filterable = GetComponent<Filterable>();
			InitializeStatusItems();
		}

		public override void OnSpawn()
		{
			base.OnSpawn();

			Building building = GetComponent<Building>();
			inputCell = building.GetUtilityInputCell();
			outputCell = building.GetUtilityOutputCell();

			Conduit.GetFlowManager(Type).AddConduitUpdater(ConduitUpdate);

			OnFilterChanged(ElementLoader.FindElementByHash(FilteredElement).tag);
			filterable.onFilterChanged += new Action<Tag>(OnFilterChanged);
			GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Main, filterStatusItem, this);

			TeleStorageData.Instance.storageContainers.Add(this);
		}

		public override void OnCleanUp()
		{
			Conduit.GetFlowManager(Type).RemoveConduitUpdater(ConduitUpdate);
			TeleStorageData.Instance.storageContainers.Remove(this);
			base.OnCleanUp();
		}

		private bool IsValidFilter {
			get {
				return (FilteredTag != null) && (FilteredElement != SimHashes.Void) && (FilteredElement != SimHashes.Vacuum);
			}
		}

		private bool IsOperational {
			get {
				return IsValidFilter && GetComponent<Operational>().IsOperational;
			}
		}

		public void FireRefresh()
		{
			try {
				Trigger(-1697596308);
			} catch (Exception ex) {
				Debug.LogWarning(ex.ToString());
			}
		}

		private void OnFilterChanged(Tag tag)
		{
			FilteredTag = tag;
			Element element = ElementLoader.GetElement(FilteredTag);
			if (element != null) {
				FilteredElement = element.id;
			}
			GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, !IsValidFilter, null);
		}

		[OnDeserialized]
		private void OnDeserialized()
		{
			if (ElementLoader.GetElement(FilteredTag) == null)
				return;
			filterable.SelectedTag = FilteredTag;
			OnFilterChanged(FilteredTag);
		}

		private void InitializeStatusItems()
		{
			filterStatusItem ??= new StatusItem("Filter", "BUILDING", "", StatusItem.IconType.Info, NotificationType.Neutral, false, OverlayModes.LiquidConduits.ID, true, 129022) {
				resolveStringCallback = (str, data) => {
					if (data is not TeleStorage infiniteSource || infiniteSource.FilteredElement == SimHashes.Void) {
						str = string.Format(STRINGS.BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, STRINGS.BUILDINGS.PREFABS.GASFILTER.ELEMENT_NOT_SPECIFIED);
					} else {
						Element elementByHash = ElementLoader.FindElementByHash(infiniteSource.FilteredElement);
						str = string.Format(STRINGS.BUILDINGS.PREFABS.GASFILTER.STATUS_ITEM, elementByHash.name);
					}
					return str;
				},
				conditionalOverlayCallback = new Func<HashedString, object, bool>(ShowInUtilityOverlay)
			};
		}

		private bool ShowInUtilityOverlay(HashedString mode, object data)
		{
			return Type switch {
				ConduitType.Gas => mode == OverlayModes.GasConduits.ID,
				ConduitType.Liquid => mode == OverlayModes.LiquidConduits.ID,
				_ => false,
			};
		}

		private void ConduitUpdate(float dt)
		{
			ConduitFlow flowManager = Conduit.GetFlowManager(Type);
			if (flowManager == null) {
				return;
			}
			ConduitFlow.ConduitContents inputContents = flowManager.GetContents(inputCell);
			TeleStorageData.AddOrUpdateStored(Type, inputContents.element, (element, inputStored) => {
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
			TeleStorageData.AddOrUpdateStored(Type, FilteredElement, (element, outputStored) => {
				float possibleOutput = Math.Min(outputStored.mass, Flow / TeleStorageFlowControl.GramsPerKilogram);
				if (possibleOutput > 0.0f) {
					var delta = flowManager.AddElement(outputCell, FilteredElement, possibleOutput, outputStored.temperature, 0, 0);
					outputStored.mass -= delta;
					TeleStorageData.Instance.FireRefresh();
				}
				return outputStored;
			});
		}

		public int AddStorageItems(CollapsibleDetailContentPanel targetPanel, int num = 0)
		{
			foreach (KeyValuePair<SimHashes, StoredItem> pair in TeleStorageData.GetStoredElements(Type)) {
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
					builder.Append($" state {elementObj.state}");
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
