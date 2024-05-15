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
    public bool remoteExists = true;

    // Remember to set instance = this to avoid getting a NullReference error.
    private void Awake() {
        Instance = this;
    }
    public void Pickup(string itemName, GameObject picture=null, Material materialPicture=null) {

        switch (itemName) {

            case "RemoteControl":

                if (remoteControlTransform != null) {
                    firstOpenInventory = 1;
                    InventoryManager.Instance.AddItem(remoteControlItem);
                    remoteExists = false;
                }

                break;

            case "Key":

                if (keyTransform != null) {

                    firstOpenInventory = 1;
                    picture.GetComponent<MeshRenderer>().materials[1].CopyPropertiesFromMaterial(materialPicture);
                    InventoryManager.Instance.AddItem(keyItem);
                    Destroy(keyTransform.gameObject);
                } else {
                    // The key doesn't exists 
                }

                break;
            default:
                break;
        }
                

    }


}
