using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using UnityEngine;

public class HandleQuota : MonoBehaviour {

    
    // Holds random values for each level in a chapter
    // At the end of a chapter it will have to be reset to an empty array for the next chapter --> done in CheckChapter.cs
    public static List<MapRandomValues> mapRandomValuesList = new List<MapRandomValues>();

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
    }
}