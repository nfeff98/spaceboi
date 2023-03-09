using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePlayerController : MonoBehaviour {
    // This Instance is called a property, not a variable, we changee its value automatically through the get and set functions
    public static SimplePlayerController Instance { get; private set; }

    public event EventHandler<OnSelectedResourceChangedEventArgs> OnSelectedResourceChanged;
    public class OnSelectedResourceChangedEventArgs : EventArgs {
        public InteractableResource selectedResource;
    }

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Animator spaceBoiAnim;
    [SerializeField] private LayerMask resourceLayerMask;


    private bool isWalking;
    private Vector3 lastInteractDir;
    private InteractableResource selectedResource;

    private void Awake() {
        if (Instance != null) {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (selectedResource != null) {
            selectedResource.Interact();
        }
    }

    private void Update() {
        HandleMovement();
        HandleInteractions();
    }

    public bool IsWalking() {
        return isWalking;
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

    private void HandleMovement() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove) {
            // Cannot move towards moveDir

            // Attempt only x movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove) {
                // Can move only on the X
                moveDir = moveDirX;
            } else {
                // Cannot move only on the X

                // Attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

                if (canMove) {
                    // Can move only on the z
                    moveDir = moveDirZ;
                }
            }
        }

        if (canMove) {
            transform.position += moveDir * moveDistance;
        }

        isWalking = moveDir != Vector3.zero;
        spaceBoiAnim.SetBool("walking", moveDir.magnitude > 0.01f);

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }


    private void SetSelectedResource(InteractableResource selectedResource) {
        this.selectedResource = selectedResource;

        OnSelectedResourceChanged?.Invoke(this, new OnSelectedResourceChangedEventArgs {
            selectedResource = selectedResource
        });
    }

}
