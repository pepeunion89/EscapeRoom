using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour{

    public static ItemPickup Instance {  get; private set; }

    public ItemSO item;
    public int firstOpenInventory = 0;
    [SerializeField] public Transform keyTransform;
    public bool keyExists = true;

    // Remember to set instance = this to avoid getting a NullReference error.
    private void Awake() {
        Instance = this;
    }
    public void Pickup() {

        if(keyTransform != null) {
            firstOpenInventory = 1;
            InventoryManager.Instance.AddItem(item);
            Destroy(keyTransform.gameObject);
        }

    }


}
