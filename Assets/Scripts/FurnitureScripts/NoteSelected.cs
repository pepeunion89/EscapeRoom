using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteSelected : MonoBehaviour{

    public static NoteSelected Instance { get; private set; }
    public bool noteReaded = false;
    
    //[SerializeField] private InteractableObject interactableObject;
    [SerializeField] public GameObject visualGameObjectNote;

    private void Awake() {
        if (Instance != null) {
            Debug.Log("More than one instance NoteSelected.");
        }
        Instance = this;
    }

    public void ShowNote() {
        if(noteReaded == false) {
            noteReaded = true;

            BookcaseScript.Instance.resizeBookcase();
        }
        visualGameObjectNote.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HideNote() {
        visualGameObjectNote.SetActive(false);
        Time.timeScale = 1f;
    }


}
