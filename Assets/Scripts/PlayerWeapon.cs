using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public  WeaponType  weaponType          = WeaponType.Invalid;
    public  int         damage;
    public  int         currentLoadedAmmo;
    public  int         magazineSize;
    public  int         currentSpareAmmo;
    public  int         maxAmmo;
    public  int         shotCount;
    public  int         burstCount;
    public  int         burstIndex;
    public  int         burstFireRate;
    public  int         falloffDamage;
    public  float       critMultiplier;
    public  float       falloffStart;
    public  float       falloffMax;
    public  float       maxRange;
    public  float       fireRate; 
    public  float       currentSpread; 
    public  float       minSpread;
    public  float       maxSpread; 
    public  float       spreadIncrease; 
    public  float       spreadRecovery;
    public  float       movementSpread;
    public  float       reloadTime;
    public  bool        allowContinuousFire; 
    public  bool        reloading;
    public  bool        readyToShoot; 
    public  bool        shooting;

    public  BurstInfo   burstInfo;
    public  GameObject  model;

    public GameObject GetModel()
    {
        return model;
    }

    public enum WeaponType
    {
        Invalid,
        Sidearm,
        SMG
    }

    public struct BurstInfo 
    {
        public  int     burstCount;
        public  int     burstIndex;
        public  int     burstFireRate;
        public  float   burstSpread;
    }

    protected struct ProjectileInfo
    {
        
    }
}