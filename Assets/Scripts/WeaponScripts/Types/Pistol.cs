using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pistol : PlayerWeapon
{
    public Pistol()
    {
        weaponType              = WeaponType.Sidearm;
        damage                  = 20;
        currentLoadedAmmo       = 10;
        magazineSize            = 10;
        currentSpareAmmo        = -1;
        maxAmmo                 = -1;
        shotCount               = 1;
        critMultiplier          = 2.0f;
        falloffStart            = 20f;
        falloffMax              = 40f;
        falloffDamage           = 12;
        maxRange                = 200f;
        fireRate                = 10.0f;
        currentSpread           = 0.005f;
        minSpread               = 0.005f;
        maxSpread               = 0.05f;
        spreadIncrease          = 0.02f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.5f;
        reloadTime              = 1.0f;
        drawTime                = 0.3f;
        stowTime                = 0.4f;
        allowContinuousFire     = false;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        //burstInfo               = new BurstInfo();
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
    }

}
