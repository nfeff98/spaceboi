using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour {


    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingBar;
    

    public void LoadScene(int sceneId) {
        StartCoroutine(LoadSceneAsync(sceneId));
    }


    IEnumerator LoadSceneAsync(int sceneId) {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingScreen.SetActive(true);

        while (!operation.isDone) {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            loadingBar.fillAmount = progressValue;

            yield return null;
        }
    }



    private void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    /*
    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    */
}
