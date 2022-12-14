using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacle : MonoBehaviour
{
    public  bool        visible;
    private Collider    colliderComponent;
    private Material    material;
    private float       originalAlpha       = 1.0f;

    [SerializeField] private GameManagerServer.Colour objectColour;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the component has a box collider or a Mesh collider
        colliderComponent = GetComponent<Collider>();
        
        material = GetComponent<Renderer>().material;
        originalAlpha = material.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        var currentColour = GameManagerServer.GetRoundColour();

        if (currentColour == objectColour)
        {
            colliderComponent.enabled   = false;
            visible                     = false;
            var oldColour = material.color;
            material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, 0.5f));
        }
        else
        {
            colliderComponent.enabled   = true;
            visible                     = true;
            var oldColour = material.color;
            material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, originalAlpha));
        }
    }

    public GameManagerServer.Colour GetColour()
    {
        return objectColour;
    }
}
