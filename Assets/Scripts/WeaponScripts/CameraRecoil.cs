using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public  PlayerWeapon.CameraRecoilInfo mRecoilInfo;
    public  PlayerMotor playerMotor;
    private Vector3     currentRotation;
    private Vector3     rotation;

    private void FixedUpdate()
    {
        currentRotation         = Vector3.Lerp(currentRotation, Vector3.zero, mRecoilInfo.returnSpeed * Time.fixedDeltaTime);
        rotation                = Vector3.Slerp(rotation, currentRotation, mRecoilInfo.rotationSpeed * Time.fixedDeltaTime);
        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void Shoot(Camera cam)
    {
        currentRotation += new Vector3(
                                -mRecoilInfo.recoilRotation.x, 
                                Random.Range(-mRecoilInfo.recoilRotation.y, mRecoilInfo.recoilRotation.y), 
                                Random.Range(-mRecoilInfo.recoilRotation.z, mRecoilInfo.recoilRotation.z));
    }

    public void UpdateRecoilInfo(PlayerWeapon.CameraRecoilInfo recoilInfo)
    {
        mRecoilInfo = recoilInfo;
    }
}
