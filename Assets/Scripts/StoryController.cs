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
    [SerializeField] private HandleQuota handleQuota;
    private Inventory inv;

    //ref planet
    //ref map generator
    // have some generate next level function based on num rooms
    // call 

    // Start is called before the first frame update
    void Start() {
        inv = FindObjectOfType<Inventory>();
        if (newMapOnStart) // BUG HERE - INFINITE LOOPS SOMEHOW
        {
            if (CheckChapter.newChapter)
            {
                //this.resourcePlacer.ResetTotals();
                //CheckChapter.newChapter = false;
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
        
        if (CheckChapter.newChapter) {
            HandleQuota.totalChapterElysium = 0;
            HandleQuota.totalChapterStromg = 0;
            HandleQuota.totalChapterWomp = 0;
            HandleQuota.totalChapterZaza = 0;

            CheckChapter.newChapter = false;
            for (int i = 0; i < numLevels; i++) {
                float perlinSeed = HandleQuota.mapRandomValuesList[i].perlinSeed;
                Debug.Log("Perlin Seed: " + perlinSeed);
                float riverWidth = HandleQuota.mapRandomValuesList[i].riverWidth;
                float riverBendiness = HandleQuota.mapRandomValuesList[i].riverBendiness;
                float riverRotation = HandleQuota.mapRandomValuesList[i].riverRotation;
                float riverPosition = HandleQuota.mapRandomValuesList[i].riverPosition;

                float distroMatFloat = HandleQuota.mapRandomValuesList[i].distroMatValue;
                float resourceMatFloat = HandleQuota.mapRandomValuesList[i].resourceMatValue;

                this.mapGen.GenerateNewMap(perlinSeed, riverWidth, riverBendiness, riverRotation, riverPosition);
                this.resourcePlacer.PlaceNewResources(distroMatFloat, resourceMatFloat, true);
            }
           // handleQuota.SetChapterQuota();
            StartCoroutine(PrintStatements());
        }

        if (currentLevel < numLevels)
        {
            float perlinSeed = HandleQuota.mapRandomValuesList[currentLevel].perlinSeed;
            float riverWidth = HandleQuota.mapRandomValuesList[currentLevel].riverWidth;
            float riverBendiness = HandleQuota.mapRandomValuesList[currentLevel].riverBendiness;
            float riverRotation = HandleQuota.mapRandomValuesList[currentLevel].riverRotation;
            float riverPosition = HandleQuota.mapRandomValuesList[currentLevel].riverPosition;

            float distroMatFloat = HandleQuota.mapRandomValuesList[currentLevel].distroMatValue;
            float resourceMatFloat = HandleQuota.mapRandomValuesList[currentLevel].resourceMatValue;

            this.mapGen.GenerateNewMap(perlinSeed, riverWidth, riverBendiness, riverRotation, riverPosition);
            this.resourcePlacer.PlaceNewResources(distroMatFloat, resourceMatFloat, false);
            StoryController.currentLevel += 1;
            yield return new WaitForSeconds(0.2f);
            //this.resourcePlacer.ResetTotals(); // << NOT SURE WE WANT THIS, THIS WOULD MAKE IT IMPOSSIBLE TO TRACK LEVEL BY LEVEL TOTALS. RESET TOTALS SHOULD BE BETWEEN CHAPTERS
            this.animalSpawner.PlaceAnimals();
            mapGen.waterLevelPlane.GetComponent<Collider>().enabled = false;
            Debug.Log("Current level: " + StoryController.currentLevel);
            Debug.Log(HandleQuota.mapRandomValuesList[0].perlinSeed);
        }
        else
        {
            // Load Spacedoc Scene
            //sceneChanger.LoadScene()
        }
        inv.UnlockStuff();
        StartCoroutine(QuotaAppearance());
    }


    private IEnumerator PrintStatements() {
        //Debug.Log("Total Womp in Chapter: " + HandleQuota.totalChapterWomp);
        yield return new WaitForSeconds(0.7f);
        handleQuota.SetChapterQuota();
        handleQuota.UpdateQuotaAppearance();
        Debug.Log("Total Womp in Chapter: " + HandleQuota.totalChapterWomp);
        //HandleQuota.wompQuota = (int)(2.5f * HandleQuota.totalChapterWomp);
        Debug.Log("Womp Quota: " + HandleQuota.wompQuota);
        Debug.Log("Zaza Quota: " + HandleQuota.zazaQuota);
        Debug.Log("Stromg Quota: " + HandleQuota.stromgQuota);
        Debug.Log("Elysium Quota: " + HandleQuota.elysiumQuota);
       // resourcePlacer.ResetTotals();
    }

    private IEnumerator QuotaAppearance() {
        yield return new WaitForSeconds(0.7f);
        handleQuota.UpdateQuotaAppearance();
    }


    /*#if UNITY_EDITOR

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
    */
}
