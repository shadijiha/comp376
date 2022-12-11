using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BurstRifle : PlayerWeapon
{
    public BurstRifle()
    {
        weaponType              = WeaponType.BurstRifle;
        description             = "A low-recoil, precision, burst-fire rifle with a three round bursts. Spread increases moderately when moving. Has a very high critical hit modifier.";
        damage                  = 15;
        currentLoadedAmmo       = 18;
        magazineSize            = 18;
        currentSpareAmmo        = 120;
        maxAmmo                 = 120;
        shotCount               = 1;
        critMultiplier          = 1.75f;
        falloffStart            = 50f;
        falloffMax              = 100f;
        falloffDamage           = 10;
        maxRange                = 200f;
        fireRate                = 3.0f;
        currentSpread           = 0.01f;
        minSpread               = 0.01f;
        maxSpread               = 0.1f;
        spreadIncrease          = 0.01f;
        spreadRecovery          = 0.01f;
        movementSpread          = 1.0f;
        reloadTime              = 1.75f;
        drawTime                = 0.8f;
        stowTime                = 0.9f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 20f,
                                        rotationSpeed = 15f,
                                        recoilRotation = new Vector3(4f, 1.5f, 1.5f)
                                    };

        
        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 10f,
                                        rotationRecoilSpeed = 10f,
                                        positionReturnSpeed = 20f,
                                        rotationReturnSpeed = 20f,

                                        RecoilRotation      = new Vector3(-5,       5,          7),
                                        MinRecoilRotation   = new Vector3(-2.5f,    2.5f,       3.5f),
                                        RecoilKick          = new Vector3(0.1f,     0.1f,       -0.2f),
                                        MinRecoilKick       = new Vector3(0.05f,    0.05f,      -0.1f)
                                    };

        burstInfo               = new BurstInfo()
                                    {
                                        burstCount              = 3,
                                        burstIndex              = 0,
                                        burstFireRate           = 15,
                                        burstSpread             = 0.005f
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
        name                    = "Burst Rifle";
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
                        plr = hit.collider.GetComponent<Head>().par.name;
                    }
                    else
                    {
                        playerShoot.m_hitCrosshair.Hit();
                        plr = hit.collider.name;
                    }
                    
                    
                    playerShoot.CmdPlayerShot(plr, playerShoot.name, finalDamage);
                }

                // Play Hit effect on the server
                if (!hit.collider.CompareTag("Ceiling"))
                {
                    playerShoot.CmdOnHit(hit.point, hit.normal);
                }

                playerShoot.CmdOnHitLaser(playerShoot.GetComponentInChildren<ParticleOrigin>().gameObject.transform.position, hit.point, 50f);
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
