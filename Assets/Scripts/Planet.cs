using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Planet : MonoBehaviour
{


    public bool windTriggered = false;
    public bool earthquakeTriggered = false;


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
    public TextMeshProUGUI climateText;
    public Animator earthAnim;
    public ParticleSystem windPFX;
    public ParticleSystem windPFXtrail;
    public ParticleSystemForceField windForce;
    private Inventory inv;

    public void LoadRoom()
    {
        //map generation
    }

    void Start()
    {
        climateText.text = "Normal";
        inv = FindObjectOfType<Inventory>();
    }


    private void ResetClimate() {
        climate = Climate.Normal;
        climateText.text = "Normal";
    }

    public IEnumerator Earthquakes()
    {
        earthquakeTriggered = true;
        for (int i = 0; i < 6; i++) {
            float rand = Random.Range(15, 45);
            earthAnim.Play("earthquake");
            inv.UpdateDebugText("It's an earthquake!");
            //destroy some resources
            yield return new WaitForSeconds(rand);

        }

        ResetClimate();
    }

    public IEnumerator SetWindy()
    {
        windTriggered = true;
        windPFX.Stop();
        windPFXtrail.Play();
        
        for (int i = 0; i < 6; i++)
        {
            float rand = Random.Range(15, 45);
            Vector2 rand2 = Random.insideUnitCircle * 0.07f;
            FindObjectOfType<SpaceBoyController>().wind = new Vector3(rand2.x, 0, rand2.y)*60;
            windForce.directionX = rand2.x;
            windForce.directionZ = rand2.y;
            yield return new WaitForSeconds(rand);

            inv.UpdateDebugText("The wind changed!");
        }

        ResetClimate();
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
                if ((float)totalWomp / (float)totalWompCreated < climateThreshold && climate == Climate.Normal)
                {
                    climate = Climate.Windy;
                    inv.UpdateDebugText("It's getting windy...");
                    climateText.text = "<color=yellow>Windy</color>";
                    StartCoroutine(SetWindy());
                }
                break;
            case Inventory.Resource.Stromg:
                totalStromg--;
                if ((float)totalStromg / (float)totalStromgCreated < climateThreshold && climate == Climate.Normal)
                {
                    inv.UpdateDebugText("The ground is shifting...");
                    climate = Climate.Earthquakes;
                    climateText.text = "<color=red>Earthquakes</color>";
                    StartCoroutine(Earthquakes());
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
