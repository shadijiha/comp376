using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEffectDetection : MonoBehaviour
{

   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log(other);
        GetComponent<MiddleManWallEffect>().callWallEffect(other.gameObject);
    }
}
