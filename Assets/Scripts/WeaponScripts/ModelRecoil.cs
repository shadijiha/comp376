using UnityEngine;

public class ModelRecoil : MonoBehaviour

{
	[Header("Reference Points")]
	public	Transform						mBasePosition;
	public	Transform						mRotationAnchor;

	[Header("Recoil Info")]
	[SerializeField]
	public	PlayerWeapon.ModelRecoilInfo	mRecoilInfo;

	
	//public	float		positionRecoilSpeed	= 8f;
	//public	float		rotationRecoilSpeed	= 8f;
	//public	float		positionReturnSpeed	= 18f;
	//public	float		rotationReturnSpeed	= 38f;

	//[Header("Amount Settings:")]
	//public	Vector3		RecoilRotation		= new Vector3(10, 5, 7);
	//public	Vector3		MinRecoilRotation	= new Vector3(5, 2.5f, 3.5f);
	//public	Vector3		RecoilKick			= new Vector3(0.015f, 0f, -0.2f);
	//public	Vector3		MinRecoilKick		= new Vector3(0.0075f, 0f, -0.1f);

	public	Vector3		rotationRecoil;
	public	Vector3		positionRecoil;
	public	Vector3		rotation;

	private void FixedUpdate()
	{
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

		mRotationAnchor.transform.localRotation	= Quaternion.Euler(rotation);
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

		//Debug.Log($"Recoil:({recoilX}, {recoilY}, {recoilZ})");

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

		//Debug.Log($"Kick:({kickX}, {kickY}, {kickZ})");
		positionRecoil	+= new Vector3(kickX, kickY, kickZ);
	}

	public void UpdateRecoilInfo(PlayerWeapon.ModelRecoilInfo recoilInfo)
    {
		mRecoilInfo = recoilInfo;
    }
}
