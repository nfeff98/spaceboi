using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoryController;

public class CheckChapter : MonoBehaviour {

    public static bool newChapter = true;
    [SerializeField] private SceneChange sceneChanger;
    [SerializeField] private HandleQuota handleQuota;

    public void ChapterCheck() {

        // At the start of game it is chapter 1 so we need to generate values for 3 levels
        if (GameStart()) {
            handleQuota.GenerateRandomValues(3);
            //Debug.Log("Chapter Check: " + HandleQuota.mapRandomValuesList[0].perlinSeed);
            sceneChanger.LoadScene(2);
            HandleQuota.wompQuotaComplete = false;
            HandleQuota.zazaQuotaComplete = false;
            HandleQuota.stromgQuotaComplete = false;
            HandleQuota.elysiumQuotaComplete = false;
        } 
        // New chapter is starting
        else if (StoryController.numLevels != 0 && StoryController.currentLevel == StoryController.numLevels)  {
            HandleQuota.totalChapterWomp = 0;
            HandleQuota.totalChapterZaza = 0;
            HandleQuota.totalChapterStromg = 0;
            HandleQuota.totalChapterElysium = 0;

            
            // Chapter Quota Failed
            if (!handleQuota.ChapterQuotaComplete()) {
                StoryController.currentLevel = 0;
                ResetQuotaCompletion();
            } else {
                ResetQuotaCompletion();
                newChapter = true;
                switch (StoryController.currentChapter) {
                    case StoryController.Chapter.Chapter1:
                        Debug.Log("Entering Chapter 2");
                        StoryController.currentChapter = StoryController.Chapter.Chapter2;
                        handleQuota.GenerateRandomValues(5);
                        //Debug.Log("Chapter Check: " + HandleQuota.mapRandomValuesList[0].perlinSeed);
                        sceneChanger.LoadScene(2);
                        break;
                    case StoryController.Chapter.Chapter2:
                        Debug.Log("Entering Chapter 3");
                        StoryController.currentChapter = StoryController.Chapter.Chapter3;
                        handleQuota.GenerateRandomValues(7);
                        //Debug.Log("Chapter Check: " + HandleQuota.mapRandomValuesList[0].perlinSeed);
                        sceneChanger.LoadScene(2);
                        break;
                    case StoryController.Chapter.Chapter3:
                        Debug.Log("End of game reached!");
                        TriggerEnding();
                        break;
                }
                StoryController.numLevels = 0;
            }
        } else {
            sceneChanger.LoadScene(2);
        }
    }


    public void TriggerEnding() {
        sceneChanger.LoadScene(1);
    }



    private bool GameStart() {
        if (StoryController.currentChapter == StoryController.Chapter.Chapter1 && StoryController.numLevels == 0) {
            return true;
        }
        return false;
    }


    private void ResetQuotaCompletion() {
        HandleQuota.wompQuotaComplete = false;
        HandleQuota.zazaQuotaComplete = false;
        HandleQuota.stromgQuotaComplete = false;
        HandleQuota.elysiumQuotaComplete = false;
    }
}
