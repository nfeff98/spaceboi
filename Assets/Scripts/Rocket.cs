using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


    [SerializeField] private GameObject rocketFrame;
    [SerializeField] private GameObject rocketHalf;
    [SerializeField] private GameObject rocketFinal;

    private void Start() {
        rocketFrame.SetActive(false);
        rocketHalf.SetActive(false);
        rocketFinal.SetActive(false);

        switch (StoryController.currentChapter) {
            case StoryController.Chapter.Chapter1:
                if (StoryController.currentLevel == 3) {
                    rocketFrame.SetActive(true);
                }
                break;
            case StoryController.Chapter.Chapter2:
                if (StoryController.currentLevel == 5) {
                    rocketHalf.SetActive(true);
                }
                break;
            case StoryController.Chapter.Chapter3:
                if (StoryController.currentLevel == 7) {
                    rocketFinal.SetActive(true);
                }
                break;
        }
    }

}
