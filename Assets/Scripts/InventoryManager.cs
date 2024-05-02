using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryManager : MonoBehaviour {

    public static InventoryManager Instance { get; private set; }
    public List<ItemSO> Items = new List<ItemSO>();

    public Inventory ItemContent;
    public GameObject InventoryItem;
    public InventoryCell inventory;
    public InventoryCell equipped;


    private void Awake() {

        Instance = this;

    }

    private void Start() {
        inventory = ItemContent.GetInventory();
        equipped = ItemContent.GetEquiped();
    }

    public void AddItem(ItemSO item) {
        if (!Items.Contains(item)) { // Checking not duplicated
            Items.Add(item);
        }
    }

    public void ListItems() {

        if (ItemPickup.Instance.firstOpenInventory == 0) {

        } else if (ItemPickup.Instance.firstOpenInventory == 1) {
            /*
                        foreach (Transform item in ItemContent) {
                            Destroy(item.gameObject);
                        }*/

            foreach (var itemSO in Items) {
                inventory.SetCell(itemSO.itemName, itemSO.icon);
                ItemPickup.Instance.firstOpenInventory = 2;
            }
        }
    }

    public void EquippeOrUnequippe() {

        if (ItemPickup.Instance.firstOpenInventory > 0) {

            if (inventory.transform.Find("ItemName").GetComponent<Text>().text == "Key") {

                equipped.SetCell(inventory.ItemName.text, inventory.ItemIcon.sprite);
                inventory.SetCell(null, null);

            } else { 

                inventory.SetCell(equipped.ItemName.text, equipped.ItemIcon.sprite);
                equipped.SetCell(null, null);

            }

        }

    }


}
