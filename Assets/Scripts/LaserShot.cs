using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using VolumetricLines;

public class LaserShot : NetworkBehaviour
{
    private float destroyCounter;
    private float counterToAcel;
    private float accelCounter;
    private float colorTimer;

    private float veloMag;
    private Vector3 velocity;
    private Light light;
    private Color color;
    private VolumetricLineBehavior vbl;
    // Start is called before the first frame update
    void Start()
    {
        destroyCounter = 5f;
        colorTimer = destroyCounter / 2;
        counterToAcel = 0.25f;
        accelCounter = 1f;
        light = gameObject.GetComponent<Light>();
        vbl = gameObject.GetComponent<VolumetricLineBehavior>();
        //color = light.color;

        veloMag = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        velocity = gameObject.GetComponent<Rigidbody>().velocity;

    }

    // Update is called once per frame
    void Update()
    {
        destroyCounter -= Time.deltaTime;
        counterToAcel -= Time.deltaTime;
        accelCounter += Time.deltaTime;
        colorTimer -= Time.deltaTime;

        //veloMag = gameObject.GetComponent<Rigidbody>().velocity.magnitude;
        //velocity = gameObject.GetComponent<Rigidbody>().velocity;
        if (destroyCounter <= 0)
        {
            Destroy(gameObject);
        }
        if (counterToAcel <= 0)
        {
            //vbl.linecolor = new Color(255f / 255f, 25f / 255f, 50f / 255f);
            light.color = new Color(255f / 255f, 25f / 255f, 50f / 255f);
            gameObject.GetComponent<Rigidbody>().velocity += (velocity.normalized * 10f * accelCounter);
        }
        if (colorTimer <= 0)
        {
            //light.color = new Color(100f / 255f, 50f / 255f, 255f / 255f);
        }
            
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "laserCollider")
        {
            Destroy(gameObject);
        }
    }
    
    
}
