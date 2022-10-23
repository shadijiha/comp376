using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

//[RequireComponent(typeof(HealthFunctions))]
public class LandMine : NetworkBehaviour
{
    private int currentHealth;
    private int damage = 20;

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
            healthFunctions.DamagePlayer(damage, collision);
            Destroy(gameObject);
        }

    }
}