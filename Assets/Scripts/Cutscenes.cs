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
        //string videoUrl = Application.persistentDataPath + "/StreamingAssets/" + "cutscene1" + ".mp4";
        //videoPlayer.url = videoUrl;
        videoPlayer.clip = Resources.Load<VideoClip>("Cutscenes/cutscene1");
        if (StoryController.currentChapter == StoryController.Chapter.Chapter3 && (Planet.windTriggered || Planet.earthquakeTriggered)) {
            videoPlayer.clip = Resources.Load<VideoClip>("Cutscenes/cutscene_end_bad");
        } else if (StoryController.currentChapter == StoryController.Chapter.Chapter3 && !Planet.windTriggered && !Planet.earthquakeTriggered) {
            videoPlayer.clip = Resources.Load<VideoClip>("Cutscenes/cutscene_end_good.");
        }
        //videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
        videoPlayer.Play();
        videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
    }

    /*private void VideoPlayer_prepareCompleted(VideoPlayer source) {
        videoPlayer.Play();
    }
    */

    private void VideoPlayer_loopPointReached(VideoPlayer source) {
        if (StoryController.currentChapter != StoryController.Chapter.Chapter3) {
            sceneChange.LoadScene(3);
        } else {
            sceneChange.LoadScene(4);
        }
    }
}
