using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HeavySniper : PlayerWeapon
{
    public static new string description = "A slow firing sniper rifle with high damage and pinpoint accuracy when aiming down its long-range scope. Spread increases dramatically when moving. Has an extremely high critical hit modifier.";

    public  float   normalFoV               = 90f;
    public  float   normalMinSpread         = 0.05f;
    public  float   normalSpreadRecovery    = 0.005f;
    public  float   normalMovementSpread    = 1.5f;
    public  float   normalSpreadIncrease    = 0.05f;
    public  float   normalSpeedMult         = 1f;

    public  float   zoomFoV                 = 15f;
    public  float   zoomMinSpread           = 0.001f;
    public  float   zoomSpreadRecovery      = 0.01f;
    public  float   zoomMovementSpread      = 0.5f;
    public  float   zoomSpreadIncrease      = 0.05f;
    public  float   zoomSpeedMult           = 0.3f;

    private Color   visible                 = new Color(255f, 255f, 255f, 255f);
    private Color   faded                   = new Color(255f, 255f, 255f, 0f);

    public HeavySniper()
    {
        weaponType              = WeaponType.HeavySniper;
        damage                  = 80;
        currentLoadedAmmo       = 4;
        magazineSize            = 4;
        currentSpareAmmo        = 32;
        maxAmmo                 = 32;
        shotCount               = 1;
        critMultiplier          = 3.0f;
        falloffStart            = 200f;
        falloffMax              = 200f;
        falloffDamage           = 80;
        maxRange                = 200f;
        fireRate                = 0.75f;
        currentSpread           = 0.05f;
        minSpread               = normalMinSpread;
        maxSpread               = 0.10f;
        spreadIncrease          = normalSpreadIncrease;
        spreadRecovery          = normalSpreadRecovery;
        movementSpread          = normalMovementSpread;
        reloadTime              = 2.0f;
        drawTime                = 1.0f;
        stowTime                = 1.2f;
        zoomFoV                 = 15f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;
        speedMultiplier         = normalSpeedMult;

        burstInfo               = new BurstInfo();
        cameraRecoilInfo        = new CameraRecoilInfo()
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

    public override void AltFireActivate(PlayerShoot playerShoot)
    {
        if (!switchingWeapon)
        {
            if (!altFire)
            {
                playerShoot.weaponCam.fieldOfView   = zoomFoV;
                this.minSpread                      = zoomMinSpread;
                this.spreadRecovery                 = zoomSpreadRecovery;
                this.spreadIncrease                 = zoomSpreadIncrease;
                this.movementSpread                 = zoomMovementSpread;
                this.speedMultiplier                = zoomSpeedMult;

                scope.color                         = visible;

                altFire                             = true;
            }
        }
        else
        {
            AltFireDeactivate(playerShoot);
        }
    }

    public override void AltFireDeactivate(PlayerShoot playerShoot)
    {
        if (altFire)
        {
            playerShoot.weaponCam.fieldOfView = normalFoV;
            this.minSpread              = normalMinSpread;
            this.spreadRecovery         = normalSpreadRecovery;
            this.spreadIncrease         = normalSpreadIncrease;
            this.movementSpread         = normalMovementSpread;
            this.speedMultiplier        = normalSpeedMult;

            scope.color                 = faded;

            altFire                     = false;
        }
    }
}
