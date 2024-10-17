using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Item_SO item;
    public Image itemImage;
    public int itemCount = 0;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;

    private bool isSelected = false;  // Track whether the item is currently selected

    public void Init(Item_SO item)
    {
        this.item = item;
        itemImage.sprite = item.sprite;
        itemImage.gameObject.SetActive(true);
        itemNameText.text = item.itemName;
        itemQuantityText.text = itemCount.ToString();
        itemNameText.gameObject.SetActive(true);
        itemQuantityText.gameObject.SetActive(true);
    }

    public void UpdateInfo()
    {
        itemImage.sprite = item.sprite;
        itemImage.gameObject.SetActive(true);
        itemNameText.text = item.itemName;
        itemQuantityText.text = itemCount.ToString();
        itemNameText.gameObject.SetActive(true);
        itemQuantityText.gameObject.SetActive(true);
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        // Change visual indicator if selected (e.g., changing color, scaling)
        itemImage.color = selected ? Color.green : Color.white;  // Example of highlighting
    }

    public void UseItem()
    {
        if (itemCount > 0)
        {
            if (item.category == Item_SO.ItemCategory.Heal && Player.Instance.currentHealth < Player.Instance.maxHealth)
            {
                switch (item.itemName)
                {
                    case "Heal":
                        Player.Instance.HealPlayer(4);
                        break;
                }
            }

            if (item.category == Item_SO.ItemCategory.Damage)
            {
                switch (item.itemName)
                {
                    case "Damage":
                        Player.Instance.MoreDamage(2);
                        break;
                }
            }

            itemCount--;

            if (itemCount == 0)
            {
                itemImage.gameObject.SetActive(false);
                itemNameText.gameObject.SetActive(false);
                itemQuantityText.gameObject.SetActive(false);
            }
            else
            {
                UpdateInfo();
            }
        }
    }
}
