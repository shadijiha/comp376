using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleManWallEffect : MonoBehaviour
{

    private GameManagerServer.Colour parentColour;

    // Start is called before the first frame update
    void Start()
    {
        parentColour = gameObject.GetComponentInParent<LevelObstacle>().GetColour();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void callWallEffect(GameObject player)
    {
        var colourEffect = parentColour;
        switch (colourEffect)
        {
            case GameManagerServer.Colour.Blue:
                print("Call function Teleport of TBA");
                break;
            case GameManagerServer.Colour.Red:
                print("Call function boost shoting speed of player");   
                break;
            case GameManagerServer.Colour.Yellow:
                print(" Call function healOverTime of player");
                break;
            case GameManagerServer.Colour.Violet:
                print("call function explosion force of player!");
                break;
            /*case GameManagerServer.Colour.Orange:
                print("call function increaseDomage of player!");
                break;*/

            default:
                print("Incorrect intelligence level.");
                break;
        }
    }
}
