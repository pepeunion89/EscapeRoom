using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public static Inventory Instance { get; private set; }

    [SerializeField] public GameObject inventoryBody;
    [SerializeField] private InventoryCell inventory;
    [SerializeField] private InventoryCell equipped;

    private void Awake() {

        Instance = this;
    }

    public void Show() {
        inventoryBody.SetActive(true);
    }

    public void Hide() {
        inventoryBody.SetActive(false);
    }

    /// <summary>
    /// Get the invneotry
    /// </summary>
    /// <returns>inveity go</returns>
    public InventoryCell GetInventory() {
        return inventory;
    }

    public InventoryCell GetEquiped() {
        return equipped;
    }
}
