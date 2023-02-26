using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int mapDim;
    public enum Climate { 
        Windy, Polluted, Earthquakes, Desert, ZazaMode
    }
    public Climate climate;

    public float disasterProbability;
    public enum Disasters
    {
        Lightning, ForestFire, Flood, Tornado, Volcano
    }

    [Header("Womp")]
    public int totalWomp;

    [Header("Zaza")]
    public int totalZaza;

    [Header("Stromg")]
    public int totalStromg;

    [Header("Wooter")]
    public int totalWooter;

    [Header("Elysium")]
    public int totalElysium;

  
    public void LoadRoom()
    {
        //map generation
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
