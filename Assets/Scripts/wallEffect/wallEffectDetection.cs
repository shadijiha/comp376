using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEffectDetection : MonoBehaviour
{
    public LevelObstacle obstacle;
   
    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponentInParent<LevelObstacle>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (TryGetComponent<MiddleManWallEffect>(out MiddleManWallEffect wallEffect) && obstacle != null && !obstacle.visible) 
        { 
            wallEffect.callWallEffect(other.gameObject);
        }
    }
}
