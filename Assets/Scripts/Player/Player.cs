using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedInteractableChangedEventArgs> OnSelectedInteractableChanged;
    public class OnSelectedInteractableChangedEventArgs : EventArgs {
        public InteractableObject selectedInteractable;
    }

    [SerializeField] private float moveSpeed = 2f;

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] public Transform objectHoldPoint;
    [SerializeField] private Image boxMessage;
    [SerializeField] public Image optionsMenu;
    [SerializeField] private Text textMessage;

    [SerializeField] private Transform cameraTransform; 

    [SerializeField] private float runningSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

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
    private bool waitingForAMessage = false;


    private void Awake() {        

        if (Instance != null) {
            Debug.Log("More than one instance player.");
        }
        Instance = this;

        if (boyTransform != null) {
            animator = boyTransform.GetComponent<Animator>();
        }

        if (animator == null) {
            Debug.LogError("No se encontró el Animator en el objeto 'Remy'.");
        }

    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnJumpAction += GameInput_OnJumpAction;
        gameInput.OnEscAction += GameInput_OnEscAction;
        gameInput.OnInventory_performed += GameInput_OnInventoryAction;
    }

    private void Update() {

        if (Time.timeScale == 1f) {
            Cursor.visible = false;
        } else {
            Cursor.visible = true;
        }

        HandleMovement();
        HandleInteractions();

    }



    private void GameInput_OnJumpAction(InputAction.CallbackContext obj) {

        if (canJump) StartCoroutine(JumpAndWait());

    }    
    

    private void GameInput_OnInteractAction(InputAction.CallbackContext context) {

        if (selectedInteractable != null) {
            selectedInteractable.Interact(selectedInteractable, context);
        }
       
    }

    private void GameInput_OnEscAction(InputAction.CallbackContext context) {
        if (NoteSelected.Instance.visualGameObjectNote.activeSelf) {
            NoteSelected.Instance.HideNote();
        } else {
            bool optionsMenuActive = optionsMenu.gameObject.activeSelf;

            optionsMenu.gameObject.SetActive(!optionsMenuActive);

            Time.timeScale = optionsMenuActive ? 1f : 0f;
        }
    }

    private void GameInput_OnInventoryAction(InputAction.CallbackContext context) {

            if (Inventory.Instance.inventoryBody.activeSelf) {

                Inventory.Instance.Hide();

            } else {

                NoteSelected.Instance.HideNote();
                Inventory.Instance.Show();
                InventoryManager.Instance.ListItems();

            }
        
    }   


    // Method to HANDLE PLAYER MOVEMENT --------------------------------------------------------------------------------------------------
    public void HandleMovement() {

        if (waitingForAMessage==false){
            if (gameInput.IsRunning()) {
                moveSpeed = runningSpeed;
            } else {
                moveSpeed = 2f;
            }

            Vector2 inputVector = gameInput.GetMovementVectorNormalized();

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 moveDir = (forward * inputVector.y) + (right * inputVector.x);

            float moveDistance = moveSpeed * Time.deltaTime;

            float playerRadius = .15f;
            float playerHeight = 2f;

            bool canMove = !Physics.CapsuleCast(
                transform.position,
                transform.position + Vector3.up * playerHeight,
                playerRadius,
                moveDir,
                moveDistance
            );

            if (canMove) {

                transform.position += moveDir * moveDistance;

                transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
            }

            isWalking = moveDir != Vector3.zero;
            SetWalking(isWalking);

            isRunning = gameInput.IsRunning();
            SetRunning(isRunning);
        } else {
            // Waiting to stop showing message
        }

        
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


    public void SetWalking(bool isWalking) {

        if (animator != null) {
            animator.SetBool(Constants.AnimParamaters.IsWalking, isWalking);
        }

    }

    public void SetRunning(bool isRunning) {

        if (animator != null) {
            animator.SetBool(Constants.AnimParamaters.IsRunning, isRunning);
        }

    }

    public bool IsWalking() {
        return isWalking;
    }

    // Method to HANDLE PLAYER INTERACTIONS --------------------------------------------------------------------------------------------------

    private void HandleInteractions() {

        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * inputVector.y) + (right * inputVector.x);

        if (moveDir != Vector3.zero) {
            lastInteractDirection = moveDir;
        }

        float interactDistance = 0.5f;
        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance, layerMask)) {
            if (raycastHit.transform.TryGetComponent(out InteractableObject interactableObject)) {

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

    private void SetSelectedInteractable(InteractableObject interactableObject) {
        this.selectedInteractable = interactableObject;


        OnSelectedInteractableChanged?.Invoke(this, new OnSelectedInteractableChangedEventArgs {
            selectedInteractable = selectedInteractable
        });
    }

    public IEnumerator ShowBoxMessage(string message, int messageID, InteractableObject interactable, GameObject redHotImage = null, Material redHotMaterial = null) {

        if (boxMessage != null) {
            if (messageID == 2) {
                waitingForAMessage = true; 
            }

            boxMessage.gameObject.SetActive(true);
            textMessage.text = message;

            yield return new WaitForSeconds(2.5f);

            boxMessage.gameObject.SetActive(false);

            if(messageID == 2 &&  ItemPickup.Instance.keyExists && TVScript.Instance.tv.activeSelf) {

                // First we broke de picture
                boxMessage.gameObject.SetActive(true);
                textMessage.text = "Sorry guys, I need to see inside you";
                ItemPickup.Instance.keyExists = false;
                yield return new WaitForSeconds(3f);

                // Now we pick the key
                ItemPickup.Instance.Pickup("Key", redHotImage, redHotMaterial);
                boxMessage.gameObject.SetActive(false);

                // Now we shot the message saying that we have the key
                boxMessage.gameObject.SetActive(true);
                textMessage.text = "I take the key!";
                yield return new WaitForSeconds(2.5f);
                boxMessage.gameObject.SetActive(false);
                waitingForAMessage = false;
            }

            if(waitingForAMessage == true) {
                waitingForAMessage = false;
            }


            if (messageID == 7) {
                Destroy(interactable.gameObject);
            }

            textMessage.text = "";
        } else {
            Debug.LogError("boxMessage it is not assigned.");
        }

    }

    public void HideBoxMessage() {
        boxMessage.gameObject.SetActive(false);
    }



}
