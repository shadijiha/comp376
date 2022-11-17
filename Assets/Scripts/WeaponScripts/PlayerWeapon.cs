using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerWeapon
{
    public  WeaponType          weaponType          = WeaponType.Invalid;
    public  int                 damage;
    public  int                 currentLoadedAmmo;
    public  int                 magazineSize;
    public  int                 currentSpareAmmo;
    public  int                 maxAmmo;
    public  int                 shotCount;
    public  int                 burstCount;
    public  int                 burstIndex;
    public  int                 burstFireRate;
    public  int                 falloffDamage;
    public  float               critMultiplier;
    public  float               falloffStart;
    public  float               falloffMax;
    public  float               maxRange;
    public  float               fireRate; 
    public  float               currentSpread; 
    public  float               minSpread;
    public  float               maxSpread; 
    public  float               spreadIncrease; 
    public  float               spreadRecovery;
    public  float               movementSpread;
    public  float               reloadTime;
    public  float               drawTime;
    public  float               stowTime;
    public  bool                allowContinuousFire; 
    public  bool                reloading;
    public  bool                readyToShoot; 
    public  bool                shooting;

    public  CameraRecoilInfo    cameraRecoilInfo;
    public  ModelRecoilInfo     modelRecoilInfo;
    public  BurstInfo?          burstInfo;
    public  GameObject          model;

    public GameObject GetModel()
    {
        return model;
    }

    public enum WeaponType
    {
        Invalid,
        Sidearm,
        SMG,
        Shotgun,
        Rifle
    }

    [System.Serializable]
    public struct BurstInfo 
    {
        public  int     burstCount;
        public  int     burstIndex;
        public  int     burstFireRate;
        public  float   burstSpread;
    }

    [System.Serializable]
    public struct CameraRecoilInfo
    {
        public  float   rotationSpeed;
        public  float   returnSpeed;
        public  Vector3 recoilRotation;
    }

    [System.Serializable]
    public struct ModelRecoilInfo
    {
	    public	float		positionRecoilSpeed;
	    public	float		rotationRecoilSpeed;
	    public	float		positionReturnSpeed;
	    public	float		rotationReturnSpeed;
	    public	Vector3		RecoilRotation;
	    public	Vector3		MinRecoilRotation;
	    public	Vector3		RecoilKick;
	    public	Vector3		MinRecoilKick;
    }

    [System.Serializable]
    protected struct ProjectileInfo
    {
        
    }
}