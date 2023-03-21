using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour {
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private SpaceBoyController spaceBoyController;

    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;

    private const string SPEAKER_TAG = "speaker";

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one Dialogue Manager in the scene");
        }
        instance = this;
    }
    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start() {
        Debug.Log("Dialogue Manager Start");
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update() {
        if (!dialogueIsPlaying) {
            return;
        } else {
            if (Input.GetButtonDown("Interact")) {
                ContinueStory();
            }
        }
        
    }

    public void EnterDialogueMode(TextAsset inkJSON) {
        if (!dialogueIsPlaying) {
            currentStory = new Story(inkJSON.text);
            dialogueIsPlaying = true;
            dialoguePanel.SetActive(true);

            // Crude fix for now, will figure out a better one later
            if (!spaceBoyController.nearSpacedoc) {
                ContinueStory();
            }
        }
    }

    private IEnumerator ExitDialogueMode() {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
    }

    private void ContinueStory() {
        //Debug.Log("Continue story called");
        if (currentStory.canContinue) {
            dialogueText.text = currentStory.Continue();
            //Debug.Log("Can continue is true");
            HandleTags(currentStory.currentTags);
        } else {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void HandleTags(List<string> currentTags) {
        foreach (string tag in currentTags) {
            string[] splitTag = tag.Split(':');
            if (splitTag.Length != 2) {
                Debug.LogError("Tag could not be appropritely parsed: " + tag);
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch (tagKey) {
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                default:
                    Debug.LogWarning("Tag came in but is not currently beinig handled: " + tag);
                    break;
            }
        }
    }
}
