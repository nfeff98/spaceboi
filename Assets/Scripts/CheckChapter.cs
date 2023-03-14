using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StoryController;

public class CheckChapter : MonoBehaviour {

    public static bool newChapter = true;
    [SerializeField] private SceneChange sceneChanger;

    public void ChapterCheck() {
        if (StoryController.numLevels != 0 && StoryController.currentLevel == StoryController.numLevels)  {
            newChapter = true;
            switch(StoryController.currentChapter) {
                case StoryController.Chapter.Chapter1:
                    Debug.Log("Entering Chapter 2");
                    StoryController.currentChapter = StoryController.Chapter.Chapter2;
                    sceneChanger.LoadScene(4);
                    break;
                case StoryController.Chapter.Chapter2:
                    Debug.Log("Entering Chapter 3");
                    StoryController.currentChapter = StoryController.Chapter.Chapter3;
                    sceneChanger.LoadScene(4);
                    break;
                case StoryController.Chapter.Chapter3:
                    Debug.Log("End of game reached!");
                    TriggerEnding();
                    break;
            }
            StoryController.numLevels = 0;
        } else {
            sceneChanger.LoadScene(2);
        }
    }


    public void TriggerEnding() {
        // Different if conditions to trigger different ending scenes
        sceneChanger.LoadScene(5);
    }
}
