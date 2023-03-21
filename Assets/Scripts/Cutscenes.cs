using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class Cutscenes : MonoBehaviour {


    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private SceneChange sceneChange;

    [SerializeField] private VideoClip badEnding;
    [SerializeField] private VideoClip goodEnding;

    private void Start() {
        if (StoryController.currentChapter == StoryController.Chapter.Chapter3 && (Planet.windTriggered || Planet.earthquakeTriggered)) {
            videoPlayer.clip = badEnding;
        } else if (StoryController.currentChapter == StoryController.Chapter.Chapter3 && !Planet.windTriggered && !Planet.earthquakeTriggered) {
            videoPlayer.clip = goodEnding;
        }
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source) {
        if (StoryController.currentChapter != StoryController.Chapter.Chapter3) {
            sceneChange.LoadScene(3);
        } else {
            sceneChange.LoadScene(4);
        }
    }
}
