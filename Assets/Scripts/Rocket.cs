using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {


    [SerializeField] private GameObject rocketFrame;
    [SerializeField] private GameObject rocketHalf;
    [SerializeField] private GameObject rocketFinal;
    [SerializeField] private HandleQuota handleQuota;

    private void Start() {
        switch (StoryController.currentChapter) {
            case StoryController.Chapter.Chapter1:
                if (StoryController.currentLevel == 3 && handleQuota.ChapterQuotaComplete()) {
                    rocketFrame.SetActive(true);
                }
                break;
            case StoryController.Chapter.Chapter2:
                if (StoryController.currentLevel == 5 && handleQuota.ChapterQuotaComplete()) {
                    rocketFrame.SetActive(false);
                    rocketHalf.SetActive(true);
                } else {
                    rocketFrame.SetActive(true);
                }
                break;
            case StoryController.Chapter.Chapter3:
                if (StoryController.currentLevel == 7 && handleQuota.ChapterQuotaComplete()) {
                    rocketHalf.SetActive(false);
                    rocketFinal.SetActive(true);
                } else {
                    rocketHalf.SetActive(true);
                }
                break;
        }
    }

}
