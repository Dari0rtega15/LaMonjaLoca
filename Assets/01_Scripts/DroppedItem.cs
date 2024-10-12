using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item_SO item;
    public float time2Destroy = 10;

    void Start()
    {
        Destroy(gameObject, time2Destroy);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            InventoryManager.Instance.AddItem(item);
            Destroy(gameObject);
            Debug.Log("Se recogio el item.");
        }
    }
}
