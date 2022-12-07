using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redWallEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boostPlayerDomage(GameObject player)
    {

        player.GetComponent<WeaponManager>().AmplifyDamage();

    }
}
