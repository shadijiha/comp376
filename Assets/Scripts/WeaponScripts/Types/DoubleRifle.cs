using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DoubleRifle : PlayerWeapon
{
    public DoubleRifle()
    {
        weaponType              = WeaponType.DoubleRifle;
        damage                  = 18;
        currentLoadedAmmo       = 8;
        magazineSize            = 8;
        currentSpareAmmo        = 40;
        maxAmmo                 = 40;
        shotCount               = 2;
        critMultiplier          = 1.5f;
        falloffStart            = 50f;
        falloffMax              = 100f;
        falloffDamage           = 12;
        maxRange                = 200f;
        fireRate                = 2.0f;
        currentSpread           = 0.01f;
        minSpread               = 0.01f;
        maxSpread               = 0.03f;
        spreadIncrease          = 0.01f;
        spreadRecovery          = 0.01f;
        movementSpread          = 2.0f;
        reloadTime              = 2.0f;
        drawTime                = 0.9f;
        stowTime                = 1.1f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 20f,
                                        rotationSpeed = 25f,
                                        recoilRotation = new Vector3(6f, 2.5f, 2.5f)
                                    };

        
        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 10f,
                                        rotationRecoilSpeed = 10f,
                                        positionReturnSpeed = 20f,
                                        rotationReturnSpeed = 20f,

                                        RecoilRotation      = new Vector3(-10,      5,          7),
                                        MinRecoilRotation   = new Vector3(-5f,      2.5f,       3.5f),
                                        RecoilKick          = new Vector3(0.1f,     0.1f,       -0.4f),
                                        MinRecoilKick       = new Vector3(0.05f,    0.05f,      -0.2f)
                                    };

        burstInfo               = new BurstInfo()
                                    {
                                        burstCount              = 2,
                                        burstIndex              = 0,
                                        burstFireRate           = 8,
                                        burstSpread             = 0.01f
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
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
                            float falloffPercent = (hit.distance - falloffStart) / (falloffMax - falloffStart);
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
        currentSpread += burstInfo.burstSpread;

        if (currentSpread > maxSpread)
        {
            currentSpread = maxSpread;
        }

        // Consume ammunition
        --currentLoadedAmmo;
        ++burstInfo.burstIndex;

        playerShoot.weaponSound.Play();


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
