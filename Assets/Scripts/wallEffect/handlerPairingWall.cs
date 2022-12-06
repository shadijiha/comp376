using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handlerPairingWall : MonoBehaviour
{
    public teleportScript[] listBlueWall;
    private int originalNumberWall;
    
    

    public GameObject tempPlayer;
    // Start is called before the first frame update
    void Start()
    {
        listBlueWall = GameObject.FindObjectsOfType<teleportScript>();
        originalNumberWall = listBlueWall.Length;
        checkAllPartnerWall();
    }

    // Update is called once per frame
    void Update()
    {
   
        if (Input.GetKeyDown("y"))
        {
            Debug.Log(listBlueWall[0].gameObject.name);
            Debug.Log(originalNumberWall);
           
        }
    }

    private void checkAllPartnerWall()
    {
        for(int indexWall = 0; indexWall < originalNumberWall; indexWall++)
        {
            if (!listBlueWall[indexWall].setpartnerPosition())
            {
                Debug.Log("This wall does not have a partner");
            }

            else
            {
                listBlueWall[indexWall].getPartnerPossTest();
            }
        }
    }

    /*
        private void wallPairing()
        {
            int currentNumberWall = originalNumberWall;
            int randomIndex;
            int nextIndex = -2; //used if the first index point to an empty cell.
            GameObject[] pairingList = new GameObject[3];
            foreach(teleportScript element in listBlueWall){
                Debug.Log(element.gameObject.name);
            }

            int emergencyBreak = 0;
            while (currentNumberWall > 0)
            {

                for (int i = 0; i < 2; i++)
                {
                    randomIndex = Random.Range(1, originalNumberWall);
                    randomIndex--;
                    if (listBlueWall[randomIndex] != null)
                    {
                        pairingList[i] = listBlueWall[randomIndex].gameObject;
                        listBlueWall[randomIndex] = null;
                        currentNumberWall--;

                        Debug.Log("paired wall numbr" + currentNumberWall);
                    }
                    else // find the next available gameObject
                    {
                        nextIndex = findNextIndex(randomIndex);
                        if (nextIndex == -1) //return -1 if the function was not able to  find  a valid GameObject
                        {
                            Debug.Log("problem When assigning the pairing");
                        }
                        else
                        {
                            pairingList[i] = listBlueWall[nextIndex].gameObject;
                            listBlueWall[nextIndex] = null;
                            currentNumberWall--;

                            Debug.Log("paired wall numbr" + currentNumberWall);
                        }


                    }
                    if (nextIndex == -1) //return -1 if the function was not able to  find  a valid GameObject
                    {
                        Debug.Log("problem When assigning the pairing");
                    }
                    nextIndex = -2;
                }
                if(currentNumberWall == 1)
                {
                    nextIndex = findNextIndex(0); // find the last wall
                    if (nextIndex == -1) //return -1 if the function was not able to  find  a valid GameObject
                    {
                        Debug.Log("problem When assigning the pairing");
                    }
                    else
                    {
                        pairingList[2] = listBlueWall[nextIndex].gameObject;
                        pairingList[2] = null;
                    }
                }
               for ( int y = 0; y < pairingList.Length;y++)
               {
                    pairingList[y] = null;
               }

                emergencyBreak++;
                if(emergencyBreak == 100)
                {
                    currentNumberWall = 0;
                }

            }
            foreach (teleportScript element in listBlueWall)
            {
                Debug.Log(element);
            }




        }
    */

    private int findNextIndex(int originalIndex)
    {

        int changingIndex = originalIndex;
        changingIndex++;
        // make sure to not overflow the array index;
        if (changingIndex == originalNumberWall)
        {
            changingIndex = 0;
        }

        while (changingIndex != originalIndex)
        {
            if(listBlueWall[changingIndex] != null)
            {
                return changingIndex;
            }
            else
            {
                changingIndex++;
            }

            if (changingIndex == originalNumberWall)
            {
                changingIndex = 0;
            }
            
        }
        // return -1 if their is no other object left in the list
        return -1; 
    }
}
