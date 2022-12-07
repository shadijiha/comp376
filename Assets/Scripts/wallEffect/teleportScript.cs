using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleportScript : MonoBehaviour
{
    // Start is called before the first frame update
    public teleportScript wallPartner;
    private Vector3 partnerPosition;
    private bool activeTeleporter = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Is call when the game start to initialise every wall and remove the risk of forgetting to toggle off a wall that does not have a partner.
    // Check it the wall have a partner and set the boolean in consequences.
    public bool setpartnerPosition()
    {
        if (wallPartner)
        {
            partnerPosition = wallPartner.gameObject.transform.position;
            activeTeleporter = true;
            return true;
        }
        else
        {
            activeTeleporter = false;
            return false;
        }
    }

    public  void teleportPlayer(GameObject player)
    {
        // check to make sure the wall have a partner
        if (activeTeleporter )
        {
            //check if the player can teleport  start the timer if they can.
            if (player.GetComponent<playerWallCoolDown>().handleTeleportCoolDown())
            {
                player.transform.position = partnerPosition;
            }
            
        }

    }

    public bool getActiveTeleporter()
    {
        return activeTeleporter;
    }

    public void getPartnerPossTest()
    {
        Debug.Log(partnerPosition);
    }
}
