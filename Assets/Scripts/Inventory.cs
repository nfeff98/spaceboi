using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update

    public enum Tool
    {
        Axe, Scythe, Pickaxe, Bucket, ChainsawDozer, Harvester, Dynamite, Vacuum
    };

    public List<GameObject> tools;

    public enum Resource
    {
        Womp, Zaza, Stromg, Wooter, Elysium, Diamond
    };

    public List<InventorySlot> resourceSlots;
    public List<InventorySlot> toolSlots;
    public List<InventorySlot> toolMirrorSlots;
    public bool foundDiamond;
    public int itemCap = 20;

    private bool[] unlocked = {false, false, false, false, false, false}; //down to 6 because removing water

    [Tooltip("0 = Womp, 1 = Zaza, 2 = Stromg, 3 = Wooter, 4 = Elysium")]
    public static int[] resourceCounts = {0, 0, 0, 0, 0};
    public Tool equippedTool;
    public GameObject inventoryScreen;
    public TextMeshProUGUI debugText;
    private int pickUps;
    private bool routineRunning;
    private Resource prevPickup;
    private int pickupTimeCap = 3;
    private int pickupCounter;
    public GameObject selector;
    public GameObject dynamitePrefab;
    public GameObject dozerPrefab;
    public GameObject harvesterPrefab;
    public SpaceBoyController player;
    private GameObject activeVehicle;
    private Tool prevTool;
    public Tutorial tutorial;

    private void Awake()
    {
       // DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        inventoryScreen.SetActive(false);
        debugText.gameObject.SetActive(false);

        //default to axe
       // selector.transform.position = toolSlots[0].transform.position;
        equippedTool = Tool.Axe;
        prevTool = Tool.Scythe;
        tools[0].SetActive(true);
        player = FindObjectOfType<SpaceBoyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) ||
            Input.GetKeyDown(KeyCode.Alpha4) ||
            Input.GetKeyDown(KeyCode.Alpha5) ||
            Input.GetKeyDown(KeyCode.Alpha6)/* ||
            Input.GetKeyDown(KeyCode.Alpha7) ||
            Input.GetKeyDown(KeyCode.Alpha8)*/) SetEquipped(); //down to 6 because remove water

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeInHierarchy);
        }
        
    }

    public void UnlockStuff()
    {
        unlocked = new bool[6]; //set to 6 for no wtaer
        switch (StoryController.currentChapter)
        {
            case StoryController.Chapter.Chapter1:
                unlocked[0] = true;
                unlocked[1] = true;
                unlocked[2] = false;
                unlocked[3] = false;
                unlocked[4] = false;
                unlocked[5] = false;
                break; 
            case StoryController.Chapter.Chapter2:
                unlocked[0] = true;
                unlocked[1] = true;
                unlocked[2] = true;
                unlocked[3] = false;
                unlocked[4] = false;
                unlocked[5] = false;
                break;
            case StoryController.Chapter.Chapter3:
                unlocked[0] = true;
                unlocked[1] = true;
                unlocked[2] = true;
                unlocked[3] = true;
                unlocked[4] = true;
                unlocked[5] = true;
                break;

        }
        for (int i =0; i < 6; i++)
        {
            if (!unlocked[i])
            {
                toolSlots[i].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f);
            } else
            {
                toolSlots[i].GetComponent<Image>().color = Color.white;
            }
        }
    }

    public void AddToInventory(Resource r)
    {
        switch (r)
        {
            case (Resource.Womp):
                resourceCounts[0]++;
                /*
                if (Tutorial.newGame && !DialogueManager.GetInstance().dialogueIsPlaying) {
                    DialogueManager.GetInstance().EnterDialogueMode(tutorial.tutorialInk2);
                    Tutorial.newGame = false;
                }*/
                break;
            case (Resource.Zaza):
                resourceCounts[1]++;

                break;
            case (Resource.Stromg):
                resourceCounts[2]++;

                break;
            case (Resource.Wooter):
                resourceCounts[3]++;

                break;
            case (Resource.Elysium):
                resourceCounts[4]++;

                break;
            case (Resource.Diamond):
                foundDiamond = true;
                break;
        }

        for (int i = 0; i < resourceSlots.Count; i++)
        {
            InventorySlot slot = (InventorySlot)resourceSlots[i];
            if (slot.count < itemCap)
            {
                if (slot.count == 0)
                {
                    slot.slotType = InventorySlot.SlotType.Resource;
                    slot.resource = r;
                    slot.count = 1;
                    slot.SetAppearance();
                    UpdateDebugText(r);
                    return;
                }
                else
                {
                    if (slot.slotType == InventorySlot.SlotType.Resource && slot.resource == r)
                    {
                        slot.count++;
                        UpdateDebugText(r);
                        slot.SetAppearance();
                        return;
                    }
                }
            }
        }

    }


    public void UpdateDebugText(Resource r)
    {
        if (!routineRunning || prevPickup != r)
        {
            pickUps = 1;
            prevPickup = r;
            routineRunning = true;
            debugText.SetText("picked up " + pickUps + " " + r.ToString());
            debugText.gameObject.SetActive(true);
            StopCoroutine(TurnOffDebugText());
            StartCoroutine(TurnOffDebugText());
        }
        else
        {
            pickUps++;
            debugText.SetText("picked up " + pickUps + " " + r.ToString());
            StopCoroutine(TurnOffDebugText());
            StartCoroutine(TurnOffDebugText());
            
        }
    }

    public void UpdateDebugText(string s)
    {
        debugText.SetText(s);
        debugText.gameObject.SetActive(true);
        StopCoroutine(TurnOffDebugText());
        StartCoroutine(TurnOffDebugText());
    }

    public IEnumerator TurnOffDebugText()
    {
        yield return new WaitForSeconds(5f);
        debugText.gameObject.SetActive(false);
        routineRunning = false;
    }

   
    public IEnumerator FlickerCollider()
    {
        this.GetComponent<SphereCollider>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        this.GetComponent<SphereCollider>().enabled = true;
    }

    public void SetEquipped(int selection = 0) //edited for down ot 6 tools
    {
        if (prevTool != equippedTool)
        {
            if (player.vehicleActive)
            {
                player.DeactivateVehicle();
                Destroy(activeVehicle);
                activeVehicle = null;
            }

            if ((Input.GetKeyDown(KeyCode.Alpha1) || selection == 1) && unlocked[0])
            {
                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                selector.transform.position = toolSlots[0].transform.position;
                equippedTool = Tool.Axe;
                tools[0].SetActive(true);

            }
            else if ((Input.GetKeyDown(KeyCode.Alpha2) || selection == 2) && unlocked[1])
            {
                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                tools[1].SetActive(true);
                selector.transform.position = toolSlots[1].transform.position;
                equippedTool = Tool.Scythe;

            }
            else if ((Input.GetKeyDown(KeyCode.Alpha3) || selection == 3) && unlocked[2])
            {
                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                tools[2].SetActive(true);
                selector.transform.position = toolSlots[2].transform.position;
                equippedTool = Tool.Pickaxe;

            }
            else if ((Input.GetKeyDown(KeyCode.Alpha4) || selection == 4) && unlocked[3])
            {
                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                //equippedTool = Tool.Bucket;
                equippedTool = Tool.ChainsawDozer;
                selector.transform.position = toolSlots[3].transform.position;
                GameObject dozer = Instantiate(dozerPrefab);
                dozer.transform.position = player.transform.position + new Vector3(0f, 2f, 0f);
                dozer.transform.rotation = player.transform.rotation;
                // don't move camera when doing this?
                player.ActivateVehicle(dozer.GetComponent<SimpleCarController>().driversSeat);
                activeVehicle = dozer;
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha5) || selection == 5) && unlocked[4])
            {
                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                equippedTool = Tool.Harvester;
                selector.transform.position = toolSlots[5].transform.position;
                GameObject harvester = Instantiate(harvesterPrefab);
                harvester.transform.position = player.transform.position + new Vector3(0f, 2f, 0f);
                harvester.transform.rotation = player.transform.rotation;
                // don't move camera when doing this?
                player.ActivateVehicle(harvester.GetComponent<SimpleCarController>().driversSeat);
                activeVehicle = harvester;
            }
            else if ((Input.GetKeyDown(KeyCode.Alpha6) || selection == 6) && unlocked[5])
            {
                

                prevTool = equippedTool;

                foreach (GameObject t in tools) t.SetActive(false);
                selector.transform.position = toolSlots[4].transform.position;
                equippedTool = Tool.Dynamite;
            }
            else if (Input.GetKeyDown(KeyCode.Alpha7) && unlocked[6])
            {

            }
            else if (Input.GetKeyDown(KeyCode.Alpha8) && unlocked[7])
            {
                //equippedTool = Tool.Vacuum;
            }
            else //default to axe
            {

            }
        }


    }
}
