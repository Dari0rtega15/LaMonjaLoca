using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    public List<ItemUI> items = new List<ItemUI>();

    private int selectedIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        if (items.Count > 0)
        {
            HighlightSelectedItem();  // Highlight the first item by default
        }
    }

    private void Update()
    {
        HandleScrollInput();
        HandleUseInput();
    }

    private void HandleScrollInput()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            // Scroll up
            selectedIndex = (selectedIndex + 1) % items.Count;
            HighlightSelectedItem();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            // Scroll down
            selectedIndex = (selectedIndex - 1 + items.Count) % items.Count;
            HighlightSelectedItem();
        }
    }

    private void HandleUseInput()
    {
        if (Input.GetKeyDown(KeyCode.F) && items[selectedIndex].itemCount > 0)
        {
            items[selectedIndex].UseItem();
        }
    }

    private void HighlightSelectedItem()
    {
        for (int i = 0; i < items.Count; i++)
        {
            items[i].SetSelected(i == selectedIndex);
        }
    }

    public void AddItem(Item_SO newItem)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != null)
            {
                if (items[i].item.itemName == newItem.itemName)
                {
                    items[i].itemCount++;
                    items[i].UpdateInfo();
                    return;
                }
            }
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item == null)
            {
                items[i].itemCount = 1;
                items[i].Init(newItem);
                return;
            }
        }
    }
}
