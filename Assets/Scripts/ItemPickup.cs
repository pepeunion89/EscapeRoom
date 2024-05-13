using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{

    public static ItemPickup Instance {  get; private set; }

    public ItemSO keyItem;
    public ItemSO remoteControlItem;
    public int firstOpenInventory = 0;
    [SerializeField] public Transform keyTransform;
    [SerializeField] public Transform remoteControlTransform;
    public bool keyExists = true;

    // Remember to set instance = this to avoid getting a NullReference error.
    private void Awake() {
        Instance = this;
    }
    public void Pickup(string itemName) {

        switch (itemName) {

            case "RemoteControl":

                if (remoteControlTransform != null) {
                    firstOpenInventory = 1;
                    InventoryManager.Instance.AddItem(remoteControlItem);
                    Destroy(remoteControlTransform.gameObject);
                }

                break;

            case "Key":

                Debug.Log("Entro en pickup Key");

                if (keyTransform != null) {

                    Debug.Log("La llave existe aun");

                    firstOpenInventory = 1;
                    InventoryManager.Instance.AddItem(keyItem);
                    Destroy(keyTransform.gameObject);
                } else {
                    Debug.Log("No existe la llave ");
                }

                break;
            default:
                break;
        }
                

    }


}
