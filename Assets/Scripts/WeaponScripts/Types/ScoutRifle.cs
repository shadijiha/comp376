using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoutRifle : PlayerWeapon
{
    public static new string description = "A rapid-firing sniper rifle with moderate damage and pinpoint accuracy when aiming down its mid-range scope. Spread remains controlled while moving. Has an extremely high critical hit modifier.";

    public  float   normalFoV               = 90f;
    public  float   normalMinSpread         = 0.02f;
    public  float   normalSpreadRecovery    = 0.0025f;
    public  float   normalMovementSpread    = 0.5f;
    public  float   normalSpreadIncrease    = 0.05f;
    public  float   normalSpeedMult         = 1f;

    public  float   zoomFoV                 = 30f;
    public  float   zoomMinSpread           = 0.001f;
    public  float   zoomSpreadRecovery      = 0.005f;
    public  float   zoomMovementSpread      = 0.25f;
    public  float   zoomSpreadIncrease      = 0.025f;
    public  float   zoomSpeedMult           = 0.5f;

    private Color   visible                 = new Color(255f, 255f, 255f, 255f);
    private Color   faded                   = new Color(255f, 255f, 255f, 0f);

    public ScoutRifle()
    {
        weaponType              = WeaponType.ScoutRifle;
        damage                  = 50;
        currentLoadedAmmo       = 8;
        magazineSize            = 8;
        currentSpareAmmo        = 64;
        maxAmmo                 = 64;
        shotCount               = 1;
        critMultiplier          = 3.0f;
        falloffStart            = 200f;
        falloffMax              = 200f;
        falloffDamage           = 60;
        maxRange                = 200f;
        fireRate                = 2.0f;
        currentSpread           = 0.02f;
        minSpread               = normalMinSpread;
        maxSpread               = 0.05f;
        spreadIncrease          = normalSpreadIncrease;
        spreadRecovery          = normalSpreadRecovery;
        movementSpread          = normalMovementSpread;
        reloadTime              = 1.5f;
        drawTime                = 0.8f;
        stowTime                = 1.0f;
        zoomFoV                 = 30f;
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
                                        recoilRotation      = new Vector3(15f,       4f,         4f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 5f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-60f,     20f,        20f),
                                        MinRecoilRotation   = new Vector3(-30f,     10f,        10f),
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
