using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacle : MonoBehaviour
{
    private BoxCollider colliderComponent;
    private Material material;
    private float originalAlpha = 1.0f;

    [SerializeField] private GameManagerServer.Colour objectColour;

    // Start is called before the first frame update
    void Start()
    {
        colliderComponent = GetComponent<BoxCollider>();
        material = GetComponent<Renderer>().material;
        originalAlpha = material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        var currentColour = GameManagerServer.GetRoundColour();
       
        // TODO: Optemize that
        if (currentColour == objectColour)
        {
            colliderComponent.enabled = false;

            var oldColour = material.color;
            material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, 0.5f));
        }
        else
        {
            colliderComponent.enabled = true;

            var oldColour = material.color;
            material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, originalAlpha));
        }
    }
}
