using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSelected : MonoBehaviour{

    public static NoteSelected Instance { get; private set; }
    
    //[SerializeField] private InteractableObject interactableObject;
    [SerializeField] public GameObject visualGameObjectNote;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("More than one instance NoteSelected.");
        }
        Instance = this;
    }

    public void ShowNote() {
        visualGameObjectNote.SetActive(true);
    }

    public void HideNote() {
        visualGameObjectNote.SetActive(false);
    }


}
