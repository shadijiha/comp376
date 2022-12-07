using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWallCoolDown : MonoBehaviour
{
    // Start is called before the first frame update
    private bool canTeleport = true;
    private int teleportCooldown;

    private bool canHealHoverTime = true;
    private int healHoverTimeCooldown;

    void Start()
    {
        teleportCooldown = 7;
        healHoverTimeCooldown = 7;



    }

  

    public bool handleGreenWallCoolDown()
    {
        if (canHealHoverTime)
        {
            canHealHoverTime = false;
            StartCoroutine(startGreenWallCoolDown());
            return true;
        }
        else
        {
            return false;
        }
    }
    IEnumerator startGreenWallCoolDown()
    {
        yield return new WaitForSeconds(healHoverTimeCooldown);
        canHealHoverTime = true;
    }


    public bool handleTeleportCoolDown()
    {
        if (canTeleport)
        {
            canTeleport = false;
            StartCoroutine(startTeleportCoolDown());
            return true;
        }
        else
        {
             return false;
        }


    }
 
    IEnumerator startTeleportCoolDown()
    {
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }
}
