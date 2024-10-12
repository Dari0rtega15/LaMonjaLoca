using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using UnityEditor.UIElements;

public class ItemUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler, IPointerClickHandler
{
    public Item_SO item;
    public Image itemImage;
    public int itemCount = 0;
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI itemQuantityText;

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

    public void OnBeginDrag(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnEndDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
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
