using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DialogueTrigger : MonoBehaviour {
    //public FrameInput Input { get; private set; }
    //private SpaceBoyController _input;

    [SerializeField] private GameInput gameInput;

    [Header("Visual Cue")]
    //[SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    public void StartDialogue() {
        if (!DialogueManager.GetInstance().dialogueIsPlaying) {
            //visualCue.SetActive(true);
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        } else {
            //visualCue.SetActive(false);
        }
    }

   /* private bool playerInRange;

    private void Start() {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) {
            //visualCue.SetActive(true);
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        } else {
            //visualCue.SetActive(false);
        }
    }

    private void Awake() {
        playerInRange = false;
        //visualCue.SetActive(false);
        //_input = GetComponent<SpaceBoyController>();
    }

    private void Update() {

    }


    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerInRange = true;
        }
    }


    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            playerInRange = false;
        }
    }
   */

}
