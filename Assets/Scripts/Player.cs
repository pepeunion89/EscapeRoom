using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class Player : MonoBehaviour, IKeyObjectParent {

    public static Player Instance { get; private set; }

    // Event to show or hide InteractableObject lightning mesh
    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
    public class OnSelectedInteractableChangedEventArgs : EventArgs {
        public InteractableObject selectedInteractable;
    }

    // Velocity player
    [SerializeField] private float moveSpeed = 2f;

    // This is the GameInput reference for the GameInput object in Unity, we instantiate gameInput to use the movement function inside.
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] public Transform keyObjectHoldPoint;
    [SerializeField] private Image boxMessage;
    [SerializeField] private Text textMessage;

    private KeyObject keyObject;

    private Animator animator;

    private Vector3 lastInteractDirection;
    private InteractableObject selectedInteractable;
    public Transform boyTransform;

    public Rigidbody rigidBody;

    [Range(0, 3)]
    public float jumpStrength = 3f;

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;

    private bool canJump = true;


    private void Awake() {
        if (Instance != null) {
            Debug.Log("More than one instance player.");
        }
        Instance = this;

        boyTransform = transform.Find("Remy");

        if (boyTransform != null) {
            animator = boyTransform.GetComponent<Animator>();
        }

        if (animator == null) {
            Debug.LogError("No se encontr? el Animator en el objeto 'Remy'.");
        }

    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
    }

    private void GameInput_OnJumpAction(InputAction.CallbackContext obj) {

        if (canJump) StartCoroutine(JumpAndWait());

    }

    private IEnumerator JumpAndWait() {

        isJumping = true;
        animator.SetBool(Constants.AnimParamaters.IsJumping, isJumping);
        canJump = false;

        rigidBody.velocity = transform.up * jumpStrength;

        yield return new WaitForSecondsRealtime(0.4f);

        isJumping = false;
        animator.SetBool(Constants.AnimParamaters.IsJumping, isJumping);
        canJump = true;


    }

    private void Update() {

        HandleMovement();
        HandleInteractions();

    }


    //private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
    private void GameInput_OnInteractAction(InputAction.CallbackContext context) {

        // Interact with an InteractableObject
        if (selectedInteractable != null) {
            selectedInteractable.Interact(selectedInteractable, context);
        }

        // ESC Keyboard pressed to manage state of the game welcome note
        if (selectedInteractable == null && NoteSelected.Instance.visualGameObjectNote.activeSelf && context.control.displayName == "Esc") {

            NoteSelected.Instance.HideNote();


        }

        // I Keyboard pressed to manage state of the game inventory
        if (context.control.displayName == "I") {

            if (Inventory.Instance.inventoryBody.activeSelf) {
                Inventory.Instance.Hide();
            } else {
                NoteSelected.Instance.HideNote();
                Inventory.Instance.Show();
                InventoryManager.Instance.ListItems();
            }

        }

    }

    public void SetWalking(bool isWalking) {

        if (animator != null) {
            animator.SetBool(Constants.AnimParamaters.IsWalking, isWalking); // Ajusta el par?metro del Animator
        }

    }

    public void SetRunning(bool isRunning) {

        if (animator != null) {
            animator.SetBool(Constants.AnimParamaters.IsRunning, isRunning);
        }

    }


    // Method to HANDLE PLAYER MOVEMENT --------------------------------------------------------------------------------------------------
    public void HandleMovement() {
        // Here we store the movement normalized on an Vector2 inputVector
        // 
        if (isRunning) {
            moveSpeed = 5f;
        } else {
            moveSpeed = 2f;
        }

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        // This is the collition code, we use Raycast to see from what position, to the destiny position, and the distance to that object
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .15f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);


        // Once we have made the collition code, we can test if the player can move or not.

        if (!canMove) {
            // If cannot move, check if Attempt to move on X
            Vector3 moveDirX = new Vector3(moveDir.x, 0f, 0);
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                //If can move, so the player can move only on X
                moveDir = moveDirX;
            } else {

                //Cannot move only on the X
                // If cannot move, check if Attempt to move on Y
                Vector3 moveDirZ = new Vector3(0, 0f, moveDir.z);
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    //If can move, so the player can move only on Z
                    moveDir = moveDirZ;
                } else {
                    // Cannot move in any direction
                }


            }

        }


        if (canMove) {
            //Multiply by deltaTime to normalize the speed
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        SetWalking(isWalking);

        isRunning = gameInput.IsRunning();
        SetRunning(isRunning);

        //Rotation and speed rotation
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


        //boyTransform.localPosition = new Vector3();
        //boyTransform.localRotation = new Quaternion();

    }

    // Method to HANDLE PLAYER INTERACTIONS --------------------------------------------------------------------------------------------------

    private void HandleInteractions() {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDirection = moveDir;
        }

        float interactDistance = 0.5f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, layerMask)) {
            if (raycastHit.transform.TryGetComponent(out InteractableObject interactableObject)) {
                //We have the interactable object
                //interactableObject.Interact(raycastHit);
                if (interactableObject != selectedInteractable) {

                    SetSelectedInteractable(interactableObject);

                }
            } else {
                SetSelectedInteractable(null);
            }
        } else {
            SetSelectedInteractable(null);
        }


    }

    public bool IsWalking() {
        return isWalking;
    }

    private void SetSelectedInteractable(InteractableObject interactableObject) {
        this.selectedInteractable = interactableObject;


        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
            selectedInteractable = selectedInteractable
        });
    }

    public IEnumerator ShowBoxMessage(string message, int messageID) {

        if (boxMessage != null) {
            boxMessage.gameObject.SetActive(true);
            textMessage.text = message;

            yield return new WaitForSeconds(2.5f);

            boxMessage.gameObject.SetActive(false);

            if(messageID == 2 &&  ItemPickup.Instance.keyExists) {

                // Here we need the code to show the picture maximized 
                boxMessage.gameObject.SetActive(true);
                textMessage.text = "Wait a minute... Is it that a key?";
                ItemPickup.Instance.keyExists = false;
                yield return new WaitForSeconds(3f);
                ItemPickup.Instance.Pickup();
                boxMessage.gameObject.SetActive(false);
            }

            textMessage.text = "";
        } else {
            Debug.LogError("boxMessage it is not assigned.");
        }

    }

    public void HideBoxMessage() {
        boxMessage.gameObject.SetActive(false);
    }

    public Transform GetKeyObjectFollowTransform() {
        return keyObjectHoldPoint;
    }

    public void SetKeyObject(KeyObject keyObject) {
        this.keyObject = keyObject;
    }

    public KeyObject GetKeyObject() {
        return keyObject;
    }

    public bool HasKeyObject() {
        return keyObject != null;
    }

    public void ClearKeyObject() {
        keyObject = null;
    }

}
