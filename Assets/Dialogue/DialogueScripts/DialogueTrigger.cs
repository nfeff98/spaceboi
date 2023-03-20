using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class DialogueTrigger : MonoBehaviour {
    //public FrameInput Input { get; private set; }
    //private SpaceBoyController _input;

    [SerializeField] private GameInput gameInput;

    [Header("ChapterEnd Ink JSON")]
    // 0 = Chapter 1 End Fail; 1 = Chapter 1 End Succeed
    // 2 = Chapter 2 End Fail; 3 = Chapter 2 End Succeed
    // 4 = Chapter 3 End Fail; 5 = Chapter 3 End Succeed
    [SerializeField] private List<TextAsset> chapterEndInkJSONList;

    [Header("Random Ink JSON")]
    [SerializeField] private List<TextAsset> randomInkJSONList;

    [System.Obsolete]
    public void StartDialogue() {
        if (StoryController.currentChapter == StoryController.Chapter.Chapter1 && StoryController.currentLevel == 0) {
            // Intro ink files play here
        } else if (StoryController.currentLevel == StoryController.numLevels && StoryController.currentLevel != 0) {
            // End chapter ink files play here
            switch (StoryController.currentChapter) {
                case StoryController.Chapter.Chapter1:
                    // if statement if they reach quota
                    break;
                case StoryController.Chapter.Chapter2:
                    break;
                case StoryController.Chapter.Chapter3:
                    break;
            }
        } else {
            if (!DialogueManager.GetInstance().dialogueIsPlaying) {
                DialogueManager.GetInstance().EnterDialogueMode(randomInkJSONList[Random.Range(0, randomInkJSONList.Count + 1)]);
            }
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
