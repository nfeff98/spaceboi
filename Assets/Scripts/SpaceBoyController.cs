using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceBoyController : MonoBehaviour {


    public static SpaceBoyController Instance { get; private set; }

    public event EventHandler<OnSelectedResourceChangedEventArgs> OnSelectedResourceChanged;
    public class OnSelectedResourceChangedEventArgs : EventArgs {
        public InteractableResource selectedResource;
    }

    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask resourceLayerMask;


    private Vector3 lastInteractDir;
    private InteractableResource selectedResource;

    private CharacterController controller;
    public Animator spaceBoiAnim;
    private Vector3 playerVelocity;
    public  bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (selectedResource != null) {
            selectedResource.Interact();
        }
    }

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }


    public void Jump()
    {
        
        if (groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
    }

    private void HandleMovement() {
        Ray ray = new Ray(this.transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 0.3f)) {
            if (hit.transform.gameObject.tag == "Navigation")
                groundedPlayer = true;
            else {
                groundedPlayer = false;
            }

        } else {
            groundedPlayer = false;
        }

        if (groundedPlayer && playerVelocity.y < 0) {
            playerVelocity.y = 0f;
        }

        Vector3 move = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal"));
        spaceBoiAnim.SetBool("walking", move.magnitude > 0.01f);
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero) {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }



    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 3f;
        float castRadius = .5f;
        if (Physics.SphereCast(transform.position, castRadius, lastInteractDir, out RaycastHit raycastHit, interactDistance, resourceLayerMask)) {
            if (raycastHit.transform.TryGetComponent(out InteractableResource interactableResource)) {
                // Has InteractableResource
                if (interactableResource != selectedResource) {
                    SetSelectedResource(interactableResource);
                }
            } else {
                SetSelectedResource(null);
            }
        } else {
            SetSelectedResource(null);
        }
    }


    void Update() {
        HandleMovement();
        HandleInteractions();
    }


    private void SetSelectedResource(InteractableResource selectedResource) {
        this.selectedResource = selectedResource;

        OnSelectedResourceChanged?.Invoke(this, new OnSelectedResourceChangedEventArgs {
            selectedResource = selectedResource
        });
    }
}