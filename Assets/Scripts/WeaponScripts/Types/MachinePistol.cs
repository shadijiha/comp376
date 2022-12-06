using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MachinePistol : PlayerWeapon
{
    public MachinePistol()
    {
        weaponType              = WeaponType.MachinePistol;
        damage                  = 5;
        currentLoadedAmmo       = 30;
        magazineSize            = 30;
        currentSpareAmmo        = -1;
        maxAmmo                 = -1;
        shotCount               = 1;
        critMultiplier          = 1.25f;
        falloffStart            = 10f;
        falloffMax              = 25f;
        falloffDamage           = 3;
        maxRange                = 200f;
        fireRate                = 20.0f;
        currentSpread           = 0.03f;
        minSpread               = 0.03f;
        maxSpread               = 0.2f;
        spreadIncrease          = 0.005f;
        spreadRecovery          = 0.005f;
        movementSpread          = 0.25f;
        reloadTime              = 0.8f;
        drawTime                = 0.4f;
        stowTime                = 0.5f;
        allowContinuousFire     = true;
        reloading               = false;
        readyToShoot            = true;
        shooting                = false;

        cameraRecoilInfo        = new CameraRecoilInfo()
                                    {
                                        rotationSpeed       = 8f,
                                        returnSpeed         = 20f,
                                        recoilRotation      = new Vector3(10f,       3f,         3f)
                                    };

        modelRecoilInfo         = new ModelRecoilInfo()
                                    {
                                        positionRecoilSpeed = 20f,
                                        rotationRecoilSpeed = 20f,
                                        positionReturnSpeed = 30f,
                                        rotationReturnSpeed = 40f,

                                        RecoilRotation      = new Vector3(-15,      5,          7),
                                        MinRecoilRotation   = new Vector3(-7.5f,    2.5f,       3.5f),
                                        RecoilKick          = new Vector3(0.05f,   0.02f,       -0.2f),
                                        MinRecoilKick       = new Vector3(0.025f,  0.01f,       -0.1f)
                                    };

        model                   = WeaponManager.msWeaponArr[(int)weaponType];
        name                    = "Machine Pistol";

    }
}
