using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    public GameObject inventoryPrefab;

    public int x_start;
    public int y_start;
    public int X_SPACE_BETWEEN_ITEMS;
    public int Y_SPACE_BETWEEN_ITEMS;
    public int NUMBER_OF_COLUMNS;


    public override void CreateSlots()
    {
        itemsDisplayed = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.Container.Items.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            itemsDisplayed.Add(obj, inventory.Container.Items[i]);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(x_start + (X_SPACE_BETWEEN_ITEMS * (i % NUMBER_OF_COLUMNS)), y_start + (-Y_SPACE_BETWEEN_ITEMS * (i / NUMBER_OF_COLUMNS)), 0f);
    }
}
