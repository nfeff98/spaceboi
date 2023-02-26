using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class InventorySlot : MonoBehaviour
{
    public enum SlotType { Tool, Resource };
    public SlotType slotType;

    [Tooltip("Ignored if Slot Type == Resource")]
    public Inventory.Tool tool;
    [Tooltip("Ignored if Slot Type == Tool")]
    public Inventory.Resource resource;

    public Image image;
    public int count;
    public TextMeshProUGUI counter;
    public TextMeshProUGUI placeholderText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    [ExecuteInEditMode]
    public void SetAppearance()
    {
        counter.text = count + "";
        if (slotType == SlotType.Resource)
        {
            if (count > 0)
            {
                switch (resource)
                {
                    case (Inventory.Resource.Womp):
                        placeholderText.text = "Womp";
                        break;
                    case (Inventory.Resource.Zaza):
                        placeholderText.text = "Zaza";

                        break;
                    case (Inventory.Resource.Stromg):
                        placeholderText.text = "Stromg";

                        break;
                    case (Inventory.Resource.Wooter):
                        placeholderText.text = "Wooter";

                        break;
                    case (Inventory.Resource.Elysium):
                        placeholderText.text = "Elysium";

                        break;
                    case (Inventory.Resource.Diamond):
                        placeholderText.text = "Diamond";

                        break;
                }
            } else
            {
                placeholderText.text = "empty";
            }
        } else
        {
            //switch (Inventory.Tool):
        }
    }

    private void OnGUI()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }
}
