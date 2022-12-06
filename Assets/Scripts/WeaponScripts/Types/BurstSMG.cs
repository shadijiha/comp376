using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurstSMG : PlayerWeapon
{
    public BurstSMG()
    {
        weaponType              = WeaponType.BurstSMG;
        damage                  = 8;
        currentLoadedAmmo       = 40;
        magazineSize            = 40;
        currentSpareAmmo        = 200;
        maxAmmo                 = 200;
        shotCount               = 1;
        critMultiplier          = 1.5f;
        falloffStart            = 20f;
        falloffMax              = 40f;
        falloffDamage           = 6;
        maxRange                = 200f;
        fireRate                = 5.0f;
        currentSpread           = 0.015f;
        minSpread               = 0.015f;
        maxSpread               = 0.05f;
        spreadIncrease          = 0.002f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.2f;
        reloadTime              = 1.2f;
        drawTime                = 0.5f;
        stowTime                = 0.6f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        burstInfo               = new BurstInfo()
                                    {
                                        burstCount              = 5,
                                        burstIndex              = 0,
                                        burstFireRate           = 25,
                                        burstSpread             = 0.002f
                                    };

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        rotationSpeed       = 10f,
                                        returnSpeed         = 15f,
                                        recoilRotation      = new Vector3(4f,       2f,         0f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 30f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-15,      5,          7),
                                        MinRecoilRotation   = new Vector3(-7.5f,    2.5f,       3.5f),
                                        RecoilKick          = new Vector3(0.025f,   0.01f,      -0.1f),
                                        MinRecoilKick       = new Vector3(0.0125f,  0.005f,     -0.05f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
        name                    = "Burst SMG";
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
