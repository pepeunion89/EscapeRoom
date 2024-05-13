using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcaseScript : MonoBehaviour{
   
    public static BookcaseScript Instance { get; private set; }
    public GameObject bookcase;
    private void Awake() {
        Instance = this;
    }

    public void resizeBookcase() {
        BoxCollider boxCollider = bookcase.GetComponent<BoxCollider>();
        boxCollider.size = new Vector3(0.81f, 1.11f, 0.19f);
    }


}
