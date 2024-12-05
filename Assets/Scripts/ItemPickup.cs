using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item ID")]
    [SerializeField] int itemID;

    [Header("Add-Ons")]
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] ItemCheck itemCheck;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            switch (itemID)
            {
                case 0:
                    Debug.Log("Health Aquired");
                    playerHealth.AddHealth();
                    break;
                case 1:
                    Debug.Log("Dash Aquired");
                    itemCheck.ActivateDash();
                    break;
                default:
                    Debug.LogError("No Item Aquired");
                    break;
            }
        }

        Destroy(gameObject);
    }
}
