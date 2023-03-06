using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Start is called before the first frame update
    
    public int mapDim;
    public enum Climate { 

        Normal, Windy, Polluted, Earthquakes, Desert, ZazaMode
    }
    public Climate climate;

    public float disasterProbability;
    public enum Disasters
    {
        Lightning, ForestFire, Flood, Tornado, Volcano
    }

    [Header("Womp")]
    public int totalWompCreated;
    public int totalWomp;

    [Header("Zaza")]
    public int totalZazaCreated;
    public int totalZaza;

    [Header("Stromg")]
    public int totalStromgCreated;
    public int totalStromg;

    [Header("Wooter")]
    public int totalWooterCreated;
    public int totalWooter;

    [Header("Elysium")]
    public int totalElysiumCreated;
    public int totalElysium;

    public float climateThreshold;

  
    public void LoadRoom()
    {
        //map generation
    }

    void Start()
    {
        
    }

    public void AddResource(Inventory.Resource type)
    {
        switch (type)
        {
            case Inventory.Resource.Elysium:
                totalElysium++;
                totalElysiumCreated++;
                break;
            case Inventory.Resource.Zaza:
                totalZazaCreated++;
                totalZaza++;
                break;
            case Inventory.Resource.Wooter:
                totalWooterCreated++;
                totalWooter++;
                break;
            case Inventory.Resource.Womp:
                totalWompCreated++;
                totalWomp++;

                break;
            case Inventory.Resource.Stromg:
                totalStromgCreated++;
                totalStromg++;
                break;
        }
    }

    public void SubtractResource(Inventory.Resource type)
    {
        switch (type)
        {
            case Inventory.Resource.Elysium:
                totalElysium--;
                break;
            case Inventory.Resource.Zaza:
                totalZaza--;
                break;
            case Inventory.Resource.Wooter:
                totalWooter--;
                break;
            case Inventory.Resource.Womp:
                totalWomp--;
                if (totalWomp / totalWompCreated < climateThreshold && climate == Climate.Normal)
                {
                    climate = Climate.Windy;
                    FindObjectOfType<Inventory>().UpdateDebugText("It's getting windy...");
                }
                break;
            case Inventory.Resource.Stromg:
                totalStromg--;
                if (totalStromg / totalStromgCreated < climateThreshold && climate == Climate.Normal)
                {
                    FindObjectOfType<Inventory>().UpdateDebugText("The ground is shifting...");
                    climate = Climate.Earthquakes;
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
