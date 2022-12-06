﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class greenWallEffect : MonoBehaviour
{

    private int healAmount = 50;
    private int secondOfHealing = 5;
    private int healPerSecond = 0;
    // Start is called before the first frame update
    void Start()
    {
        healPerSecond = healAmount / secondOfHealing;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void healPlayerHoverTime(GameObject player) {

        if (player.GetComponent<playerWallCoolDown>().handleGreenWallCoolDown())
        {
            StartCoroutine(healPlayer(player.GetComponent<Player>()));
        }
    
    }

    IEnumerator healPlayer( Player playerScritRef)
    {
        uint healPerTick = (uint) healPerSecond;
        for (int healingTick = 0; healingTick <secondOfHealing; healingTick++)
        {
            yield return new WaitForSeconds(1);
            playerScritRef.HoverHealBy(healPerTick);
            Debug.Log("heal Tick : " + healingTick);

        }
       
    }
}