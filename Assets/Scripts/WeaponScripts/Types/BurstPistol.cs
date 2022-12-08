using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurstPistol : PlayerWeapon
{ 
    public BurstPistol()
    {
        weaponType              = WeaponType.BurstPistol;
        description             = "A stable, high accuracy sidearm which fires in 3 round bursts. Spread remains controlled when moving. Has a moderate critical modifier.";
        damage                  = 12;
        currentLoadedAmmo       = 15;
        magazineSize            = 15;
        currentSpareAmmo        = -1;
        maxAmmo                 = -1;
        shotCount               = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 20f;
        falloffMax              = 40f;
        falloffDamage           = 8;
        maxRange                = 200f;
        fireRate                = 4.0f;
        currentSpread           = 0.02f;
        minSpread               = 0.02f;
        maxSpread               = 0.05f;
        spreadIncrease          = 0.01f;
        spreadRecovery          = 0.0025f;
        movementSpread          = 0.4f;
        reloadTime              = 1.0f;
        drawTime                = 0.3f;
        stowTime                = 0.4f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        burstInfo               = new BurstInfo()
                                    {
                                        burstCount              = 3,
                                        burstIndex              = 0,
                                        burstFireRate           = 12,
                                        burstSpread             = 0.005f
                                    };

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed         = 20f,
                                        rotationSpeed       = 10f,
                                        recoilRotation      = new Vector3(7f,       1f,         0f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 20f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-30,      10,         15),
                                        MinRecoilRotation   = new Vector3(-15f,     5f,         7.5f),
                                        RecoilKick          = new Vector3(0.05f,    0.02f,      -0.25f),
                                        MinRecoilKick       = new Vector3(0.025f,   0.01f,     -0.125f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
        name                    = "Burst Pistol";
    }

    // Burst Shooting Behavior override
    public override (string, float) Shoot(PlayerShoot playerShoot)
    {
        // If there are multiple shots to fire, cast them all at once
        for (int shotNumber = 0; shotNumber < shotCount; ++shotNumber)
        {
            // We are shooting call shoot method on Server
            playerShoot.CmdOnShoot();

            Vector3 shotDirection = UnityEngine.Random.insideUnitSphere *
                                    (currentSpread + currentSpread * movementSpread * playerShoot.m_fMovement) +
                                    playerShoot.cam.transform.forward;

            shotDirection.Normalize();

            RaycastHit hit;
            if (Physics.Raycast(playerShoot.cam.transform.position, shotDirection, out hit, maxRange, playerShoot.mask))
            {
                // We hit Something
                if (hit.collider.CompareTag(PlayerShoot.PLAYER_TAG) || hit.collider.CompareTag(PlayerShoot.PLAYER_HEAD_TAG))
                {
                    int finalDamage = damage;
                    string plr;
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

                    if (hit.collider.CompareTag(PlayerShoot.PLAYER_HEAD_TAG))
                    {
                        finalDamage = Mathf.RoundToInt(damage * critMultiplier);
                        playerShoot.m_hitCrosshair.Crit();
                        plr = hit.collider.GetComponentInParent<Player>().name;
                    }
                    else
                    {
                        playerShoot.m_hitCrosshair.Hit();
                        plr = hit.collider.name;
                    }
                    
                    
                    playerShoot.CmdPlayerShot(plr, playerShoot.name, finalDamage);
                }

                // Play Hit effect on the server
                playerShoot.CmdOnHit(hit.point, hit.normal);
                playerShoot.CmdOnHitLaser(playerShoot.GetComponentInChildren<ParticleOrigin>().gameObject.transform.position, hit.point, 8f);
            }
        }

        //Adjust spread
        currentSpread += burstInfo.burstSpread;

        if (currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }

        // Consume ammunition
        --currentLoadedAmmo;
        ++burstInfo.burstIndex;

        if (burstInfo.burstIndex < burstInfo.burstCount)
        {
            playerShoot.cameraRecoil.Shoot(playerShoot.cam);
            playerShoot.modelRecoil.Shoot();
            playerShoot.UpdateCrosshair();

            // Indicate the weapon is ready to fire again after the appropriate delay
            return (nameof(this.Shoot), 1.0f / (hasted ? burstInfo.burstFireRate * 1.5f : burstInfo.burstFireRate));
        }
        else
        {
            // Additional recoil after the final shot of the burst.
            for (int i = 0; i < burstInfo.burstCount; ++i)
            {
                playerShoot.cameraRecoil.Shoot(playerShoot.cam);
                playerShoot.modelRecoil.Shoot();
            }

            currentSpread += spreadIncrease;

            if (currentSpread > maxSpread)
            {
                currentSpread = maxSpread;
            }

            playerShoot.UpdateCrosshair();
            burstInfo.burstIndex = 0;

            // Indicate the weapon is ready to fire again after the appropriate delay
            return (nameof(playerShoot.ReadyToShoot), 1.0f / (hasted ? fireRate * 1.5f : fireRate));
        }
    }
}
