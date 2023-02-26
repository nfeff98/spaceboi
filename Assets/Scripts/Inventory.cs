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
  


    void Start()
    {
        inventoryScreen.SetActive(false);
        debugText.gameObject.SetActive(false);
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
            IEnumerator routine = UpdateDebugTextRoutine(r);
            routineRunning = true;
            StartCoroutine(routine);
        } else
        {
            pickupCounter = 0;
            pickUps++;
        }
    }

    public IEnumerator UpdateDebugTextRoutine(Resource r)
    {
        Debug.Log("start coroutine");
        pickUps = 1; // later change this to set timer for multipickups
        debugText.gameObject.SetActive(true);
        for (pickupCounter = 0; pickupCounter < pickupTimeCap * 100; pickupCounter++)
        {
            debugText.SetText("picked up " + pickUps + " " + r.ToString());
            yield return new WaitForSeconds(pickUps / 100f);
        }
        routineRunning = false;
        debugText.gameObject.SetActive(false);
    }


    public void SetEquipped()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && unlocked[0]) {
            equippedTool = Tool.Axe;
        } else if (Input.GetKeyDown(KeyCode.Alpha2) && unlocked[0])
        {
            equippedTool = Tool.Scythe;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && unlocked[0])
        {
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


    }
}
