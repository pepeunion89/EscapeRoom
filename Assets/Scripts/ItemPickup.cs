using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{

    public static ItemPickup Instance {  get; private set; }

    public ItemSO item;
    public int firstOpenInventory = 0;
    Transform childOrKey;

    // Remember to set instance = this to avoid getting a NullReference error.
    private void Awake() {
        Instance = this;
    }

    private void Start() {
        childOrKey = gameObject.transform.Find("Key");
    }

    public void Pickup() {

        if(childOrKey != null) {
            firstOpenInventory = 1;
            InventoryManager.Instance.AddItem(item);
            Destroy(childOrKey.gameObject);
        }

    }


}
