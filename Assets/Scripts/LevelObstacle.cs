using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObstacle : MonoBehaviour
{
    private Collider colliderComponent;
    private Material material;
    private float originalAlpha = 1.0f;

    private bool active = true;
    private Vector3 normalLocalScale;
    private Vector3 zeroScale = new Vector3 (0,0,0);

    [SerializeField] private GameManagerServer.Colour objectColour;

    // Start is called before the first frame update
    void Start()
    {
        // Check if the component has a box collider or a Mesh collider
        colliderComponent = GetComponent<Collider>();
        
        material = GetComponent<Renderer>().material;
        originalAlpha = material.color.a;
        this.normalLocalScale = this.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        var currentColour = GameManagerServer.GetRoundColour();
       
        // TODO: Optemize that
        if (currentColour == objectColour)
        {
            //colliderComponent.enabled = false;

            //var oldColour = material.color;
            //material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, 0.5f));
            if(this.active == true)
            {
                this.active = false;
                //this.gameObject.SetActive(false);
                this.transform.localScale = zeroScale;

            }
        }
        else
        {
            //colliderComponent.enabled = true;

            //var oldColour = material.color;
            //material.SetColor("_Color", new Color(oldColour.r, oldColour.g, oldColour.b, originalAlpha));
            if (this.active == false)
            {
                //this.gameObject.SetActive(true);
                this.active = true;
                this.transform.localScale = normalLocalScale;
                Debug.Log("After activating the block");

            }
        }
    }
}
