using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

   // public MouseItem mouseItem = new MouseItem();

    public InventoryObject inventory;


    public void OnTriggerEnter2D(Collider2D other)
    {
        var item = other.GetComponent<GroundItem>();
        if(item)
        {
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inventory.Save();
            Debug.Log("Inventory saved!");
        }
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            inventory.Load();
            Debug.Log("Inventory loaded!");
        }
    }

    private void OnApplicationQuit()
    {
      inventory.Container.Items = new InventorySlot[24];
    }
}
