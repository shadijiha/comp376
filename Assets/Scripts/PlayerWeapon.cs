using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public string name;
    public int damage, currentLoadedAmmo, magazineSize, currentSpareAmmo, maxAmmo, shotCount;
    public float range, fireRate, currentSpread, minSpread, maxSpread, spreadIncrease, spreadRecovery, movementSpread, reloadTime;
    public bool allowContinuousFire, reloading, readyToShoot, shooting;
    public GameObject model;
}