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
    public Inventory inv;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask resourceLayerMask;

    public List<AnimalBehavior> enemiesInRange;

    public int health = 10;
    public int energy = 50; // energy

    private Vector3 lastInteractDir;
    private InteractableResource selectedResource;

    private CharacterController controller;
    public Animator spaceBoiAnim;
    private Vector3 playerVelocity;
    public  bool groundedPlayer;
    public float playerSpeed = 2.0f;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;
    public Vector3 wind;

    public bool nearShrine;
    public Shrine nearestShrine;
    private void Start()
    {
        inv = FindObjectOfType<Inventory>();
        controller = gameObject.GetComponent<CharacterController>();
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    

    private void GameInput_OnInteractAction(object sender, EventArgs e) {
        if (nearShrine) nearestShrine.Activate();

        else if (!spaceBoiAnim.GetCurrentAnimatorStateInfo(0).IsName("twohandChop2") && !spaceBoiAnim.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            if (inv.equippedTool == Inventory.Tool.Pickaxe)
            {
                spaceBoiAnim.Play("twohandPick");

            }
            else if (inv.equippedTool == Inventory.Tool.Axe)
            {
                spaceBoiAnim.Play("twohandChop2");

            }

            if (selectedResource != null)
            {
                Vector3 newForward = selectedResource.transform.position;
                newForward.y = this.transform.position.y;
                this.transform.LookAt(newForward);
                selectedResource.Interact();
            }
            foreach (AnimalBehavior en in enemiesInRange)
            {
                StartCoroutine(SubtractHealthFromEnemy(en));
            }
        }
    }

    public IEnumerator SubtractHealthFromEnemy(AnimalBehavior en)
    {
        yield return new WaitForSeconds(0.6f);
        en.health--;
        Debug.Log("hit peng");

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
        controller.Move((move  * playerSpeed + wind) * Time.deltaTime);

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


    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent(out InteractableResource interactableResource))
            {
                // Has InteractableResource
                if (interactableResource != selectedResource)
                {
                    if ((inv.equippedTool == Inventory.Tool.Axe && interactableResource.type == Inventory.Resource.Womp)
                    || (inv.equippedTool == Inventory.Tool.Pickaxe && interactableResource.type == Inventory.Resource.Stromg))
                    SetSelectedResource(interactableResource);
                }
            }
        if (other.tag == "Boundary")
        {
            inv.UpdateDebugText("Probably shouldn't go any further...");
        }

        if (other.tag == "Enemy")
        {
            enemiesInRange.Add(other.GetComponent<AnimalBehavior>());
        }

        if (other.TryGetComponent(out Shrine n))
        {
            nearShrine = true;
            nearestShrine = n;

        }

       
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent(out InteractableResource interactableResource))
        {
            // Has InteractableResource
            if (interactableResource == selectedResource)
            {
                SetSelectedResource(null);
            }
        }

        if (other.tag == "Enemy")
        {
            AnimalBehavior e;
            if (other.TryGetComponent<AnimalBehavior>(out e))
            {
                if (enemiesInRange.Contains(e))
                    enemiesInRange.Remove(e);
            }
        }

        if (other.GetComponent<Shrine>() != null)
        {
            nearShrine = false;
        }
    }


    private void HandleInteractions() {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if (moveDir != Vector3.zero) {
            lastInteractDir = moveDir;
        }

        float interactDistance = 1f;
        float castRadius = 3f;
        //TODO replace this with a simple sphere collider in front of the guy 
        // selected = closest to him within this area
        if (Physics.SphereCast(transform.position + new Vector3(0,0.5f,0), castRadius, transform.forward, out RaycastHit raycastHit, interactDistance, resourceLayerMask)) {
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
       // HandleInteractions();

        if (health <= 0)
        {
            // you died, trigger scene transition back to spacedoc area
        }
    }


    private void SetSelectedResource(InteractableResource selectedResource) {
        this.selectedResource = selectedResource;

        OnSelectedResourceChanged?.Invoke(this, new OnSelectedResourceChangedEventArgs {
            selectedResource = selectedResource
        });
    }
}