using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : PlayerWeapon
{
    public RocketManager manager;

    public RocketLauncher()
    {
        weaponType              = WeaponType.RocketLauncher;
        damage                  = 0;
        currentLoadedAmmo       = 4;
        magazineSize            = 4;
        currentSpareAmmo        = 0;
        maxAmmo                 = 0;
        shotCount               = -1;
        critMultiplier          = -1;
        falloffStart            = -1f;
        falloffMax              = -1f;
        falloffDamage           = -1;
        maxRange                = 200f;
        fireRate                = 1.0f;
        currentSpread           = 0.005f;
        minSpread               = 0.005f;
        maxSpread               = 0.005f;
        spreadIncrease          = 0.005f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.0f;
        reloadTime              = -1f;
        drawTime                = 0.3f;
        stowTime                = 0.4f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

       cameraRecoilInfo         = new CameraRecoilInfo()
                                    {
                                        returnSpeed         = 5f,
                                        rotationSpeed       = 30f,
                                        recoilRotation      = new Vector3(25f,       5f,         5f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 5f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-90,      25,         25),
                                        MinRecoilRotation   = new Vector3(-45f,     12.5f,      12.5f),
                                        RecoilKick          = new Vector3(0.1f,     0.1f,       -0.5f),
                                        MinRecoilKick       = new Vector3(0.05f,    0.05f,      -0.25f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
    }

    public override (string, float) Shoot(PlayerShoot playerShoot)
    {
        //Debug.Log("Rocket Shoot.");
        if (manager == null)
        {
            manager = model.GetComponent<RocketManager>();
        }

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

            Vector3 projectileDir = hit.point - playerShoot.GetComponentInChildren<ParticleOrigin>().gameObject.transform.position;
            manager.Shoot(playerShoot.name, projectileDir.normalized);
        }

        playerShoot.cameraRecoil.Shoot(playerShoot.cam);
        playerShoot.modelRecoil.Shoot();
            
        // Consume ammunition
        --currentLoadedAmmo;
        --manager.m_rocketsLeft;

        // Must return the method to invoke, as invoke isn't possible without being a unity behavior.
        return (nameof(playerShoot.ReadyToShoot), 1.0f / fireRate);
    }
}
