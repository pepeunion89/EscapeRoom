using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVScript : MonoBehaviour {

    public static TVScript Instance { get; private set; }
    public GameObject tv;

    private void Awake() {
        Instance = this;
    }

    public void turnOnTV() {

        tv.SetActive(true);

    }


}
