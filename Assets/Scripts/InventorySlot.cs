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

    public List<Sprite> texs;

    // Start is called before the first frame update
    void Start()
    {
        SetAppearance();
    }

    [ExecuteInEditMode]
    public void SetAppearance()
    {
        if (counter != null)
            counter.text = count + "";
        if (slotType == SlotType.Resource)
        {
            if (count > 0)
            {
                image.enabled = true;
                switch (resource)
                {
                    case (Inventory.Resource.Womp):
                        placeholderText.text = "Womp";
                        image.sprite = texs[0];
                        break;
                    case (Inventory.Resource.Zaza):
                        placeholderText.text = "Zaza";
                        image.sprite = texs[1];

                        break;
                    case (Inventory.Resource.Stromg):
                        placeholderText.text = "Stromg";
                        image.sprite = texs[2];

                        break;
                    case (Inventory.Resource.Wooter):
                        placeholderText.text = "Wooter";
                        image.sprite = texs[3];
                        break;
                    case (Inventory.Resource.Elysium):
                        placeholderText.text = "Elysium";
                        image.sprite = texs[4];

                        break;
                    case (Inventory.Resource.Diamond):
                        placeholderText.text = "Diamond";
                        image.sprite = texs[5];

                        break;
                }
            } else
            {
                placeholderText.text = "Empty";
                image.enabled = false;
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
