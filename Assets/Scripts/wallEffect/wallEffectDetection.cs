using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallEffectDetection : MonoBehaviour
{
    [SerializeField] private LevelObstacle obstacle;
   
    // Start is called before the first frame update
    void Start()
    {
        obstacle = GetComponentInParent<LevelObstacle>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (TryGetComponent<MiddleManWallEffect>(out MiddleManWallEffect wallEffect) && !obstacle.visible) 
        { 
            wallEffect.callWallEffect(other.gameObject);
        }
    }
}
