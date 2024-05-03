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
    public GameObject keyObject;
    private GameObject keyObjectInstance;


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

                keyObjectInstance = Instantiate(keyObject, Player.Instance.keyObjectHoldPoint);

                keyObjectInstance.transform.localPosition = new Vector3(0f, 0f, 0.05f);
                keyObjectInstance.transform.localRotation = Quaternion.Euler(180f, 160f, 80f);
                keyObjectInstance.transform.localScale = new Vector3(5f, 5f, 5f);


                inventory.SetCell(null, null);

            } else {

                Destroy(keyObjectInstance);

                inventory.SetCell(equipped.ItemName.text, equipped.ItemIcon.sprite);
                equipped.SetCell(null, null);

            }

        }

    }


}
