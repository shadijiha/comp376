using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public  float       rotationSpeed  = 6;
    public  float       returnSpeed    = 25;
    public  Vector3     RecoilRotation = new Vector3(2f, 2f, 2f);
    public  PlayerMotor playerMotor;
    private Vector3     currentRotation;
    private Vector3     rotation;

    private void FixedUpdate()
    {
        currentRotation         = Vector3.Lerp(currentRotation, Vector3.zero, returnSpeed * Time.fixedDeltaTime);
        rotation                = Vector3.Slerp(rotation, currentRotation, rotationSpeed * Time.fixedDeltaTime);
        //rb.MoveRotation(Quaternion.Euler(rotation));
        //playerMotor.AddRotation(rotation);
        transform.localRotation = Quaternion.Euler(rotation);
    }

    public void Shoot(Camera cam)
    {
        currentRotation += new Vector3(
                                -RecoilRotation.x, 
                                Random.Range(-RecoilRotation.y, RecoilRotation.y), 
                                Random.Range(-RecoilRotation.z, RecoilRotation.z));
        //cam.transform.rotation = Quaternion.Euler(currentRotation);
        //this.cam = cam;
    }

    public void UpdateRotationInfo(PlayerWeapon.CameraRecoilInfo recoilInfo)
    {
        this.rotationSpeed  = recoilInfo.rotationSpeed;
        this.returnSpeed    = recoilInfo.returnSpeed;
        this.RecoilRotation = recoilInfo.recoilRotation;
    }
}
