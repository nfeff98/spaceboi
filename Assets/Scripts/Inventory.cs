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

    public bool[] unlocked = {false, false, false, false, false, false, false, false};

    [Tooltip("0 = Womp, 1 = Zaza, 2 = Stromg, 3 = Wooter, 4 = Elysium")]
    public int[] resourceCounts = {0, 0, 0, 0, 0};
    public Tool equippedTool;
    public GameObject inventoryScreen;
    public TextMeshProUGUI debugText;
    private int pickUps;
    private bool routineRunning;
    private Resource prevPickup;
    private int pickupTimeCap = 3;
    private int pickupCounter;
    public GameObject selector;
  


    void Start()
    {
        inventoryScreen.SetActive(false);
        debugText.gameObject.SetActive(false);

        //default to axe
       // selector.transform.position = toolSlots[0].transform.position;
        equippedTool = Tool.Axe;
        tools[0].SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) ||
            Input.GetKeyDown(KeyCode.Alpha2) ||
            Input.GetKeyDown(KeyCode.Alpha3) ||
            Input.GetKeyDown(KeyCode.Alpha4) ||
            Input.GetKeyDown(KeyCode.Alpha5) ||
            Input.GetKeyDown(KeyCode.Alpha6) ||
            Input.GetKeyDown(KeyCode.Alpha7) ||
            Input.GetKeyDown(KeyCode.Alpha8)) SetEquipped();

        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryScreen.SetActive(!inventoryScreen.activeInHierarchy);
        }
        
    }

    public void AddToInventory(Resource r)
    {
        switch (r)
        {
            case (Resource.Womp):
                resourceCounts[0]++;
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

   


    public void SetEquipped()
    {
        foreach (GameObject t in tools) t.SetActive(false);
        if (Input.GetKeyDown(KeyCode.Alpha1) && unlocked[0]) {
            selector.transform.position = toolSlots[0].transform.position;
            equippedTool = Tool.Axe;
            tools[0].SetActive(true);
            
        } else if (Input.GetKeyDown(KeyCode.Alpha3) && unlocked[2])
        {
            equippedTool = Tool.Scythe;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && unlocked[1])
        {
            tools[1].SetActive(true);
            selector.transform.position = toolSlots[1].transform.position;
            equippedTool = Tool.Pickaxe;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && unlocked[0])
        {
            equippedTool = Tool.Bucket;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && unlocked[0])
        {
            equippedTool = Tool.ChainsawDozer;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && unlocked[0])
        {
            equippedTool = Tool.Harvester;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && unlocked[0])
        {
            equippedTool = Tool.Dynamite;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && unlocked[0])
        {
            equippedTool = Tool.Vacuum;
        }
        else //default to axe
        {
            
        }


    }
}
