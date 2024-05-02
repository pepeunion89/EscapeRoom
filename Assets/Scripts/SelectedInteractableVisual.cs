using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedInteractableVisual : MonoBehaviour{

    [SerializeField] private InteractableObject interactableObject;
    [SerializeField] private GameObject visualGameObject;

    private void Start() {
        Player.Instance.OnSelectedInteractableChanged += Player_OnSelectedInteractableChanged;
    }

    private void Player_OnSelectedInteractableChanged(object sender, Player.OnSelectedInteractableChangedEventArgs e) {
        if(e.selectedInteractable == interactableObject) {

            Show();

        } else {

            Hide();

        }
    }

    private void Show() {
        visualGameObject.SetActive(true);
    }

    private void Hide() {
        visualGameObject.SetActive(false);
    }
}
