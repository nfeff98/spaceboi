using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;
using TMPro;

public class HandleQuota : MonoBehaviour {

    
    // Holds random values for each level in a chapter
    // At the end of a chapter it will have to be reset to an empty array for the next chapter --> done in CheckChapter.cs
    public static List<MapRandomValues> mapRandomValuesList = new List<MapRandomValues>();
    public static List<int> chapterQuotaList = new List<int>();

    public static int wompQuota;
    public static int zazaQuota;
    public static int stromgQuota;
    public static int elysiumQuota;

    public static int totalChapterWomp;
    public static int totalChapterZaza;
    public static int totalChapterStromg;
    public static int totalChapterElysium;

    private float quotaMultiplier = 2.5f;
    private float quotaPercentage = 0.5f;

    public static bool wompQuotaComplete;
    public static bool zazaQuotaComplete;
    public static bool stromgQuotaComplete;
    public static bool elysiumQuotaComplete;

    public List<TextMeshProUGUI> elysiumCounts;
    public List<TextMeshProUGUI> zazaCounts;
    public List<TextMeshProUGUI> wompCounts;
    public List<TextMeshProUGUI> stromgCounts;

    public List<GameObject> elysiumCheckboxes;
    public List<GameObject> zazaCheckboxes;
    public List<GameObject> wompCheckboxes;
    public List<GameObject> stromgCheckboxes;


    public struct MapRandomValues {
        // Variables
        public float perlinSeed;
        public float riverWidth;
        public float riverBendiness;
        public float riverRotation;
        public float riverPosition;

        public float distroMatValue;
        public float resourceMatValue;

        // Constructor
        public MapRandomValues(float perlinSeedC, float riverWidthC, float riverBendinessC, float riverRotationC, float riverPositionC, float distroMatValueC, float resourceMatValueC) {
            perlinSeed = perlinSeedC;
            riverWidth = riverWidthC;
            riverBendiness = riverBendinessC;
            riverRotation = riverRotationC;
            riverPosition = riverPositionC;

            distroMatValue = distroMatValueC;
            resourceMatValue = resourceMatValueC;
        }
    }


    // Generates all our random values and adds them to the static list that can be accessed everywhere
    public void GenerateRandomValues(int levels) {
        mapRandomValuesList.Clear();
        for (int i = 0; i < levels; i++) {
            float perlinSeedG = Random.Range(0, 1000);
            float riverWidthG = Random.Range(0.2f, 0.4f);
            float riverBendinessG = Random.Range(-0.5f, 0.5f);
            float riverRotationG = Random.Range(-0.2f, 0.2f);
            float riverPositionG = Random.Range(-0.6f, 0.6f);

            float distroMatValueG = Random.Range(30, 180);
            float resourceMatValueG = Random.Range(0, 100);

            MapRandomValues randomValues = new MapRandomValues(perlinSeedG, riverWidthG, riverBendinessG, riverRotationG, riverPositionG, distroMatValueG, resourceMatValueG);
            mapRandomValuesList.Add(randomValues);
        }
        Debug.Log(mapRandomValuesList[0].perlinSeed);
    }

    public void Start()
    {
    }


    public void UpdateQuotaAppearance()
    {
        foreach (TextMeshProUGUI count in wompCounts) count.text = Inventory.resourceCounts[0] + " / " + wompQuota;
        foreach (TextMeshProUGUI count in zazaCounts) count.text = Inventory.resourceCounts[1] + " / " + zazaQuota;
        foreach (TextMeshProUGUI count in stromgCounts) count.text = Inventory.resourceCounts[2] + " / " + stromgQuota;
        foreach (TextMeshProUGUI count in elysiumCounts) count.text = Inventory.resourceCounts[4] + " / " + elysiumQuota;

        if (Inventory.resourceCounts[0] >= wompQuota)
        {
            wompQuotaComplete = true;
            foreach (GameObject check in wompCheckboxes)
            {
                check.SetActive(true);
            } 
        }
        else
        {
            foreach (GameObject check in wompCheckboxes)
            {
                check.SetActive(false);
            }
        }

        if (Inventory.resourceCounts[1] >= zazaQuota)
        {
            zazaQuotaComplete = true;
            foreach (GameObject check in zazaCheckboxes)
            {
                check.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject check in zazaCheckboxes)
            {
                check.SetActive(false);
            }
        }

        if (Inventory.resourceCounts[2] >= stromgQuota)
        {
            stromgQuotaComplete = true;
            foreach (GameObject check in stromgCheckboxes)
            {
                check.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject check in stromgCheckboxes)
            {
                check.SetActive(false);
            }
        }

        if (Inventory.resourceCounts[4] >= elysiumQuota)
        {
            elysiumQuotaComplete = true;
            foreach (GameObject check in elysiumCheckboxes)
            {
                check.SetActive(true);
            }
        }
        else
        {
            foreach (GameObject check in elysiumCheckboxes)
            {
                check.SetActive(false);
            }
        }
    }

    public void SetChapterQuota() {
        //chapterQuotaList.Clear();
        

        switch (StoryController.currentChapter) {
            case StoryController.Chapter.Chapter1:
                //Debug.Log(totalChapterWomp + "from handle quota");
                wompQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterWomp));
                zazaQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterZaza));
                break;
            case StoryController.Chapter.Chapter2:
                wompQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterWomp));
                zazaQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterZaza));
                stromgQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterStromg));
                elysiumQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterElysium));
                break;
            case StoryController.Chapter.Chapter3:
                wompQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterWomp));
                zazaQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterZaza));
                stromgQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterStromg));
                elysiumQuota = (int)(quotaPercentage * (quotaMultiplier * totalChapterElysium));
                break;
        }
    }


    public bool ChapterQuotaComplete() {
        if (wompQuotaComplete && zazaQuotaComplete && stromgQuotaComplete && elysiumQuotaComplete) {
            return true;
        }
        return false;
    }
}