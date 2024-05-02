using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static Player;

public class InteractableObject : MonoBehaviour {

    public void Interact(InteractableObject interactable, InputAction.CallbackContext context) {

        if (interactable.gameObject.name == "Note") {

            switch (context.control.displayName) {
                case "E":
                    Inventory.Instance.Hide();
                    NoteSelected.Instance.ShowNote();
                    break;
                case "Esc":
                    NoteSelected.Instance.HideNote();
                    break;
                default:
                    break;
            }            

        }

        if(interactable.gameObject.name == "PictureRedHot") {

            Debug.Log("Entro en RedHot");

            ItemPickup.Instance.Pickup();

        }

        if (interactable.gameObject.name == "Door") {

            if(InventoryManager.Instance.equipped.transform.Find("ItemName").GetComponent<Text>().text == "Key") {

                //Change the Transform position of the door and handle elements to enable the way out.

                //First we must obtain the references to work better.
                GameObject door_2 = (interactable.gameObject.transform.Find("Door_2").gameObject).transform.Find("Door_2").gameObject;

                GameObject door_element_27 = door_2.transform.Find("Door_Element_27").gameObject;
                GameObject handle_7 = door_2.transform.Find("Handle_7").gameObject;
                GameObject handle_8 = door_2.transform.Find("Handle_8").gameObject;


                //Now that we have the references, we can change the Transform parameters of each one easier
                door_element_27.gameObject.transform.localPosition = new Vector3(-0.371f, -0.454f, -0.046f);
                door_element_27.gameObject.transform.localRotation = Quaternion.Euler(180f, 0f, 90f);
                door_element_27.gameObject.GetComponent<BoxCollider>().enabled = true;

                handle_7.gameObject.transform.localPosition = new Vector3(-0.416f, -0.462f, -0.037f);
                handle_7.gameObject.transform.localRotation = Quaternion.Euler(6.537f, -90f, 90f);


                handle_8.gameObject.transform.localPosition = new Vector3(-0.366f, -0.462f, -0.037f);
                handle_8.gameObject.transform.localRotation = Quaternion.Euler(180f, 90f, 90f);


                interactable.gameObject.GetComponent<BoxCollider>().enabled = false;

            } else {

                //You dont have the key
                
            }

        }

    }

}
