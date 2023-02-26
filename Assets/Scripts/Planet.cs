using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Chapter { Chapter1, Chapter2, Chapter3 };
    public Chapter chapter;
    private int numRooms;
    private int roomsCleared;
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
    public bool WompEnabled;
    public int totalWomp;

    [Header("Zaza")]
    public bool ZazaEnabled;
    public int totalZaza;

    [Header("Stromg")]
    public bool StromgEnabled;
    public int totalStromg;

    [Header("Wooter")]
    public bool WooterEnabled;
    public int totalWooter;

    [Header("Elysium")]
    public bool ElysiumEnabled;
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
