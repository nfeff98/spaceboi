using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {


    public static bool newGame = true;
    public static bool zazaPickedUp = false;
    [SerializeField] private TextAsset tutorialInk1;
    [SerializeField] public TextAsset tutorialInk2;
    [SerializeField] public TextAsset eatZazaTutorial;

    private void Start() {
        if (newGame) {
            Debug.Log("Tutorial Start");
            if (!DialogueManager.GetInstance().dialogueIsPlaying) {
                Debug.Log("Dialouge Start");
                DialogueManager.GetInstance().EnterDialogueMode(tutorialInk1);
            }
        }
    }
}
