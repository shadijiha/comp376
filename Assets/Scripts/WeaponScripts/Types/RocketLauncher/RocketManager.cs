using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RocketManager : MonoBehaviour
{
    public  int         m_rocketsLeft   = 4;
    public  Rocket[]    m_rocketObjects;

    public void Shoot(string source, Vector3 dir, PlayerShoot playerShoot)
    {
        Debug.Log("RocketManagerShoot");
        m_rocketObjects[4 - m_rocketsLeft].m_direction = dir;
        m_rocketObjects[4 - m_rocketsLeft].Launch(source, playerShoot);
    }
}
