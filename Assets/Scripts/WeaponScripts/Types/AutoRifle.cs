using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AutoRifle : PlayerWeapon
{
    public AutoRifle()
    {
        weaponType              = WeaponType.AutoRifle;
        description             = "A low-recoil, precision, fully-automatic rifle with a moderate rate of fire. Spread increases dramatically when moving. Has a very high critical hit modifier.";
        damage                  = 12;
        currentLoadedAmmo       = 30;
        magazineSize            = 30;
        currentSpareAmmo        = 180;
        maxAmmo                 = 180;
        shotCount               = 1;
        critMultiplier          = 1.75f;
        falloffStart            = 50f;
        falloffMax              = 100f;
        falloffDamage           = 8;
        maxRange                = 200f;
        fireRate                = 8.0f;
        currentSpread           = 0.01f;
        minSpread               = 0.01f;
        maxSpread               = 0.075f;
        spreadIncrease          = 0.003f;
        spreadRecovery          = 0.004f;
        movementSpread          = 1.5f;
        reloadTime              = 1.75f;
        drawTime                = 0.8f;
        stowTime                = 0.9f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        returnSpeed = 20f,
                                        rotationSpeed = 10f,
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
        name                    = "Auto Rifle";
    }
}
