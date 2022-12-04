using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class violetWallEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void boostPlayerHaste(GameObject player)
    {

        player.GetComponent<WeaponManager>().Haste();

    }
}
