using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HealthPack : NetworkBehaviour
{
    private int currentHealth;
    private int maxHealth;
    private int gain = 20;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Player"))
        {
            var healthFunctions = collision.gameObject.GetComponent<HealthFunctions>();
            healthFunctions.HealPlayer(gain, collision);
            Destroy(gameObject);

        }

    }
}
