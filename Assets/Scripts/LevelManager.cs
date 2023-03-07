using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {


    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private ResourcePlacer resourcePlacer;

    // Start is called before the first frame update
    void Start() {
        mapGenerator.GenerateNewMap();
        resourcePlacer.PlaceNewResources();
    }
}
