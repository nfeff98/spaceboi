using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Planet : MonoBehaviour
{

    public bool windTriggered = false;
    public bool earthquakeTriggered = false;
    [SerializeField]private GameObject earthquake;
    [SerializeField]private GameObject wind;



   
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
    public static int totalWompCreated;
    public static int totalWompRemaining;
    public float wompRatio;

    [Header("Zaza")]
    public static int totalZazaCreated;
    public static int totalZazaRemaining;
    public float zazaRatio;

    [Header("Stromg")]
    public static int totalStromgCreated;
    public static int totalStromgRemaining;
    public float stromgRatio;

    [Header("Wooter")]
    public static int totalWooterCreated;
    public static int totalWooterRemaining;

    [Header("Elysium")]
    public static int totalElysiumCreated;
    public static int totalElysiumRemaining;
    public float elysiumRatio;

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
        // earthquake.setActive(true);
        // wind.setActive(false);
    }


    private void ResetClimate() {
        climate = Climate.Normal;
        climateText.text = "Normal";
        earthquake.SetActive(false);
        wind.SetActive(false);
    }

    public IEnumerator Earthquakes()
    {
        earthquake.SetActive(true);
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
        wind.SetActive(true);
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
        //Debug.Log("AddResource called...");
        switch (type)
        {
            case Inventory.Resource.Elysium:
                totalElysiumRemaining++;
                totalElysiumCreated++;
                HandleQuota.totalChapterElysium++;
                break;
            case Inventory.Resource.Zaza:
                totalZazaCreated++;
                totalZazaRemaining++;
                HandleQuota.totalChapterZaza++;
                break;
            case Inventory.Resource.Wooter:
                totalWooterCreated++;
                totalWooterRemaining++;
                // No wooter quota for now...
                break;
            case Inventory.Resource.Womp:
                totalWompCreated++;
                totalWompRemaining++;
                HandleQuota.totalChapterWomp++;
                break;
            case Inventory.Resource.Stromg:
                totalStromgCreated++;
                totalStromgRemaining++;
                HandleQuota.totalChapterStromg++;
                break;
        }
    }

    public void SubtractResource(Inventory.Resource type)
    {
        switch (type)
        {
            case Inventory.Resource.Elysium:
                totalElysiumRemaining--;
                break;
            case Inventory.Resource.Zaza:
                totalZazaRemaining--;
                break;
            case Inventory.Resource.Wooter:
                totalWooterRemaining--;
                break;
            case Inventory.Resource.Womp:
                totalWompRemaining--;
                Debug.Log("Total Womp Created: " + totalWompCreated);
                Debug.Log("Total Womp Remaining: " + totalWompRemaining);
                if ((float)totalWompRemaining / (float)totalWompCreated < climateThreshold && climate == Climate.Normal)
                {
                    climate = Climate.Windy;
                    inv.UpdateDebugText("It's getting windy...");
                    climateText.text = "<color=yellow>Windy</color>";
                    StartCoroutine(SetWindy());                   
                    // wind.setActive(true);
                }
                break;
            case Inventory.Resource.Stromg:
                totalStromgRemaining--;
                if ((float)totalStromgRemaining / (float)totalStromgCreated < climateThreshold && climate == Climate.Normal)
                {
                    inv.UpdateDebugText("The ground is shifting...");
                    climate = Climate.Earthquakes;
                    climateText.text = "<color=red>Earthquakes</color>";
                    StartCoroutine(Earthquakes());
                    // earthquake.setActive(true);
                }
                break;
        }
    }

    
    // Update is called once per frame
    void Update()
    {
        wompRatio = (float)totalWompRemaining / (float)totalWompCreated;
        stromgRatio = (float)totalStromgRemaining / (float)totalStromgCreated;
    }
}
