using UnityEngine;

public class ModelRecoil : MonoBehaviour

{
	[Header("Reference Points")]
	public	Transform						mBasePosition;
	public	Transform						mRotationAnchor;
	public	Transform						mCamera;
	public	Transform						mPlayer;

	[Header("Recoil Info")]
	public	PlayerWeapon.ModelRecoilInfo	mRecoilInfo;
	public	Vector3		rotationRecoil;
	public	Vector3		positionRecoil;
	public	Vector3		rotation;

    private void Update()
    {
        float	xRot = mCamera.rotation.eulerAngles.x;
		float	yRot = mPlayer.rotation.eulerAngles.y;
		mRotationAnchor.transform.rotation = Quaternion.Euler(rotation.x + xRot, + rotation.y + yRot, rotation.z);
    }

    private void FixedUpdate()
	{
		float	xRot = mCamera.rotation.eulerAngles.x;
		float	yRot = mPlayer.rotation.eulerAngles.y;
		mRotationAnchor.transform.rotation		= Quaternion.Euler(rotation.x + xRot, + rotation.y + yRot, rotation.z);

		rotationRecoil							= Vector3.Lerp(
														rotationRecoil, 
														Vector3.zero, 
														mRecoilInfo.rotationReturnSpeed * Time.fixedDeltaTime);

		positionRecoil							= Vector3.Lerp(
														positionRecoil, 
														Vector3.zero, 
														mRecoilInfo.positionReturnSpeed * Time.fixedDeltaTime);

		mBasePosition.transform.localPosition	= Vector3.Slerp(
														mBasePosition.localPosition, 
														positionRecoil, 
														mRecoilInfo.positionRecoilSpeed * Time.fixedDeltaTime);

		rotation								= Vector3.Slerp(
														rotation, 
														rotationRecoil, 
														mRecoilInfo.rotationRecoilSpeed * Time.fixedDeltaTime);

		mRotationAnchor.transform.rotation		= Quaternion.Euler(rotation.x + xRot, + rotation.y + yRot, rotation.z);
	}

	public void Shoot()
	{
		float recoilX	= Random.Range(mRecoilInfo.MinRecoilRotation.x, mRecoilInfo.RecoilRotation.x);

		float recoilY	= Random.value * mRecoilInfo.RecoilRotation.y;
		if (recoilY		< mRecoilInfo.MinRecoilRotation.y)
        {
			recoilY		-= mRecoilInfo.RecoilRotation.y;
        }

		float recoilZ	= Random.value * mRecoilInfo.RecoilRotation.z;
		if (recoilZ		< mRecoilInfo.MinRecoilRotation.z)
        {
			recoilZ		-= mRecoilInfo.RecoilRotation.z;
        }

		rotationRecoil	+= new Vector3(recoilX, recoilY, recoilZ);

		float kickZ		= Random.Range(mRecoilInfo.MinRecoilKick.z, mRecoilInfo.RecoilKick.z);

		float kickY		= Random.value * mRecoilInfo.RecoilKick.y;
		if (kickY < mRecoilInfo.MinRecoilKick.y)
        {
			kickY		-= mRecoilInfo.RecoilKick.y;
        }

		float kickX		= Random.value * mRecoilInfo.RecoilKick.x;
		if (kickX		< mRecoilInfo.MinRecoilKick.x)
        {
			kickX		-= mRecoilInfo.RecoilKick.x;
        }

		positionRecoil	+= new Vector3(kickX, kickY, kickZ);
	}

	public void UpdateRecoilInfo(PlayerWeapon.ModelRecoilInfo recoilInfo)
    {
		mRecoilInfo = recoilInfo;
    }
}
