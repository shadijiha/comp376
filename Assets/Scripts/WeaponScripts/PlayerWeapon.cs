using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class PlayerWeapon
{
    public  WeaponType          weaponType              = WeaponType.Invalid;
    public  int                 damage;
    public  int                 currentLoadedAmmo;
    public  int                 magazineSize;
    public  int                 currentSpareAmmo;
    public  int                 maxAmmo;
    public  int                 shotCount;
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
    public  float               speedMultiplier         = 1f; 
    public  bool                allowContinuousFire;

    public  bool                reloading;
    public  bool                readyToShoot; 
    public  bool                shooting;
    public  bool                altFire                 = false;
    public  bool                hasted                  = false;
    public  bool                amplified               = false;

    public  CameraRecoilInfo    cameraRecoilInfo;
    public  ModelRecoilInfo     modelRecoilInfo;
    public  BurstInfo           burstInfo;
    public  GameObject          model;
    public  Image               scope;
   
    // Default Shooting Behavior
    virtual public (string, float) Shoot(PlayerShoot playerShoot)
    {
        // If there are multiple shots to fire, cast them all at once
        for (int shotNumber = 0; shotNumber < shotCount; ++shotNumber)
        {
            // We are shooting call shoot method on Server
            playerShoot.CmdOnShoot();

            Vector3 shotDirection = UnityEngine.Random.insideUnitSphere * 
                                    (currentSpread + currentSpread * movementSpread) + 
                                    playerShoot.cam.transform.forward;

            shotDirection.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(playerShoot.cam.transform.position, shotDirection, out hit, maxRange, playerShoot.mask))
            {
                // We hit Something
                if (hit.collider.tag == PlayerShoot.PLAYER_TAG)
                {
                    int finalDamage = damage;
                    if (hit.distance > falloffStart)
                    {
                        if (hit.distance < falloffMax)
                        {
                            // Damage linearly falls off between the minimum falloff distance and the maximum falloff.
                            float falloffPercent = (hit.distance - falloffStart)/(falloffMax - falloffStart);
                            finalDamage = Mathf.RoundToInt(
                                (falloffPercent * falloffDamage) +
                                ((1 - falloffPercent) * damage));
                        }
                        else
                        {
                            finalDamage = falloffDamage;
                        }
                    }
                    
                    playerShoot.CmdPlayerShot(hit.collider.name, playerShoot.name, finalDamage);
                }

                // Play Hit effect on the server
                playerShoot.CmdOnHit(hit.point, hit.normal);
            }
        }

        //Adjust spread
        currentSpread += spreadIncrease;

        if(currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }

        playerShoot.cameraRecoil.Shoot(playerShoot.cam);
        playerShoot.modelRecoil.Shoot();

        playerShoot.UpdateCrosshair();
            
        // Consume ammunition
        --currentLoadedAmmo;

        // Must return the method to invoke, as invoke isn't possible without being a unity behavior.
        return (nameof(playerShoot.ReadyToShoot), 1.0f / (hasted ? fireRate * 1.5f : fireRate));
    }   

    // Base behavior alt fire does nothing, exists to be overridden for special weapons
    public virtual void AltFireActivate(PlayerShoot playerShoot)
    {
        
    }

    // Base behavior alt fire does nothing, exists to be overridden for special weapons
    public virtual void AltFireDeactivate(PlayerShoot playerShoot)
    {

    }

    public virtual void Reset()
    {
        currentLoadedAmmo   = magazineSize;
        currentSpareAmmo    = maxAmmo;
        readyToShoot        = true;
        reloading           = false;
        altFire             = false;
        shooting            = false;
    }

    public GameObject GetModel()
    {
        return model;
    }

    public enum WeaponType
    {
        Invalid,
        MarksmanPistol,
        BurstPistol,
        MachinePistol,
        LightSMG,
        BurstSMG,
        HeavySMG,
        TacticalShotgun,
        AutoShotgun,
        Scattergun,
        AutoRifle,
        BurstRifle,
        DoubleRifle,
        HeavySniper,
        ScoutRifle,
        ShotRifle,
        RocketLauncher,
        FlameThrower
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