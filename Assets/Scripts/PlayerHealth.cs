using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] int health;
    [SerializeField] int healthAmt;

    [Header("Hearts")]
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite heartFull;
    [SerializeField] Sprite heartEmpty;

    private void FixedUpdate()
    {
        if(healthAmt > hearts.Length)
        {
            healthAmt = hearts.Length;
        }
        else if(healthAmt <= 0)
        {
            healthAmt = 0;
        }

        if(health > healthAmt)
        {
            health = healthAmt;
        }
        else if(health <= 0)
        {
            health = 0;
        }

        for(int i = 0; i < hearts.Length; i++)
        {
            if(i < health)
            {
                hearts[i].sprite = heartFull;
            }
            else
            {
                hearts[i].sprite = heartEmpty;
            }

            if(i < healthAmt)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

    public void AddHealth()
    {
        health += 1;
        healthAmt += 1;
    }
}
