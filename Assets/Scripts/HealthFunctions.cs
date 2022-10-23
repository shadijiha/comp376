

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HealthFunctions : NetworkBehaviour
{

    private int currentHealth;
    private int maxHealth;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HealPlayer(int amount, Collision collision)
    {

        var playerScript = collision.gameObject.GetComponent<Player>();
        currentHealth = playerScript.getHealth();
        maxHealth = playerScript.getMaxHealth();
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        playerScript.setHealth(currentHealth);

    }

    public void DamagePlayer(int amount, Collision collision)
    {
        var playerScript = collision.gameObject.GetComponent<Player>();
        currentHealth = playerScript.getHealth();
        currentHealth -= amount;
        playerScript.setHealth(currentHealth);
    }



}
