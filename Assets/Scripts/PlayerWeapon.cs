using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name = "Test Weapon";
    public int damage = 24;
    public float range = 100f;
    public float fireRate = 20.0f;
    public GameObject model;
}
