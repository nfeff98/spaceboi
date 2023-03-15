using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class StoryController : MonoBehaviour {


    public bool newMapOnStart;

    public enum Chapter { Chapter1, Chapter2, Chapter3 };
    public static Chapter currentChapter;
    public static int numLevels;
    public static int currentLevel;



    public MapGenerator mapGen;
    public ResourcePlacer resourcePlacer;
    public AnimalSpawner animalSpawner;
    

    [SerializeField] private SceneChange sceneChanger;

    //ref planet
    //ref map generator
    // have some generate next level function based on num rooms
    // call 

    // Start is called before the first frame update
    void Start() {
        if (newMapOnStart)
        {
            if (CheckChapter.newChapter)
            {
                CheckChapter.newChapter = false;
                switch (currentChapter)
                {
                    case Chapter.Chapter1:
                        numLevels = 3;
                        currentLevel = 0;
                        break;
                    case Chapter.Chapter2:
                        numLevels = 5;
                        currentLevel = 0;
                        break;
                    case Chapter.Chapter3:
                        numLevels = 7;
                        currentLevel = 0;
                        break;

                }
            }

            Debug.Log(currentChapter);
            Debug.Log(numLevels);

            LoadLevel(currentLevel);
        }
    }


    public void LoadLevel(int currentLevel) {
        StartCoroutine(LoadLevelRoutine(currentLevel));
    }

    public IEnumerator LoadLevelRoutine(int currentLevel)
    {
        if (currentLevel < numLevels)
        {
            StoryController.currentLevel += 1;
            this.mapGen.GenerateNewMap();
            yield return new WaitForSeconds(0.2f);
            this.resourcePlacer.PlaceNewResources();
            //this.resourcePlacer.ResetTotals(); // << NOT SURE WE WANT THIS, THIS WOULD MAKE IT IMPOSSIBLE TO TRACK LEVEL BY LEVEL TOTALS. RESET TOTALS SHOULD BE BETWEEN CHAPTERS
            this.animalSpawner.PlaceAnimals();
            //mapGen.waterLevelPlane.GetComponent<Collider>().enabled = false;
            Debug.Log("Current level: " + StoryController.currentLevel);
        }
        else
        {
            // Load Spacedoc Scene
            //sceneChanger.LoadScene()
        }
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(StoryController))]
    public class StoryControllerEditor : Editor {


        public override void OnInspectorGUI() {

            StoryController story = (StoryController)target;
            base.OnInspectorGUI();

            if (GUILayout.Button("New Level")) {
                story.mapGen.GenerateNewMap();
                story.resourcePlacer.PlaceNewResources();
            }
        }
    }
#endif
}
