using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class ItemPickup : MonoBehaviour
{
    [Header("Item ID")]
    [SerializeField] int itemID;

    [Header("Add-Ons")]
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] ItemCheck itemCheck;

    [Header("UI")]
    [SerializeField] GameObject collectedUI;
    PlayerControls playerControls;
    PlayerMovment playerMovment;
    PauseGame pauseGame;

    private void Awake()
    {
        collectedUI.SetActive(false);
        
        playerControls = new PlayerControls();

        playerControls.UI.Enable();
        playerControls.UI.Confirm.performed += Confirm;

        playerMovment = FindObjectOfType<PlayerMovment>();
        pauseGame = FindObjectOfType<PauseGame>();
    }

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

    private void Confirm(InputAction.CallbackContext context)
    {

    }
}
