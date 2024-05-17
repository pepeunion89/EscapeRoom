using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCell : MonoBehaviour {
    public Text ItemName;
    public Image ItemIcon;

    public void SetCell(string name, Sprite icon) {
        ItemName.text = name;
        ItemIcon.sprite = icon;
    }
}