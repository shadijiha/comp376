using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;


public class pickUpRotation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float currentAngle = 0;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(
                                    -25,
                                    currentAngle,
                                    0));

        currentAngle += 30 * Time.deltaTime;
        currentAngle = currentAngle % 360;
    }
}
