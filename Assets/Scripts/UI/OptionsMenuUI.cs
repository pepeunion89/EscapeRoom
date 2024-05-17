using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenuUI : MonoBehaviour {

    [SerializeField] private Button optionsMenu;
    public GameInput gameInput;
    public void GoToMainMenu() {

        Loader.Load(Loader.Scene.MainMenuScene);

    }

}
