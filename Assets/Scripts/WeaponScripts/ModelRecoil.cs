using UnityEngine;

public class ModelRecoil : MonoBehaviour

{
	[Header("Reference Points")]
	public	Transform	basePosition;
	public	Transform	rotationAnchor;

	[Header("Speed Settings")]
	public	float		positionRecoilSpeed	= 8f;
	public	float		rotationRecoilSpeed	= 8f;
	public	float		positionReturnSpeed	= 18f;
	public	float		rotationReturnSpeed	= 38f;

	[Header("Amount Settings:")]
	public	Vector3		RecoilRotation		= new Vector3(10, 5, 7);
	public	Vector3		RecoilKick			= new Vector3(0.015f, 0f, -0.2f);

	public	Vector3		rotationRecoil;
	public	Vector3		positionRecoil;
	public	Vector3		rotation;

	private void FixedUpdate()
	{
		rotationRecoil					= Vector3.Lerp(
												rotationRecoil, 
												Vector3.zero, 
												rotationReturnSpeed * Time.fixedDeltaTime);

		positionRecoil					= Vector3.Lerp(
												positionRecoil, 
												Vector3.zero, 
												positionReturnSpeed * Time.fixedDeltaTime);

		basePosition.transform.position		= Vector3.Slerp(
												basePosition.localPosition, 
												positionRecoil, 
												positionRecoilSpeed * Time.fixedDeltaTime);

		rotation						= Vector3.Slerp(
												rotation, 
												rotationRecoil, 
												rotationRecoilSpeed * Time.fixedDeltaTime);

		rotationAnchor.transform.rotation	= Quaternion.Euler(rotation);
	}

	public void Shoot()
	{
		Debug.Log("Shoot model recoil.");
		Debug.Log($"RotationRecoil Pre: ({rotationRecoil.x},{rotationRecoil.y},{rotationRecoil.z})");
		rotationRecoil	+= new Vector3(
							-RecoilRotation.x, 
							Random.Range(-RecoilRotation.y, RecoilRotation.y), 
							Random.Range(-RecoilRotation.z, RecoilRotation.z));
		Debug.Log($"RotationRecoil Post: ({rotationRecoil.x},{rotationRecoil.y},{rotationRecoil.z})");

		Debug.Log($"PositionRecoil Pre: ({positionRecoil.x},{positionRecoil.y},{positionRecoil.z})");
		positionRecoil	+= new Vector3(
							Random.Range(-RecoilKick.x, RecoilKick.x), 
							Random.Range(-RecoilKick.y, RecoilKick.y),
							RecoilKick.z);
		Debug.Log($"PositionRecoil Post: ({positionRecoil.x},{positionRecoil.y},{positionRecoil.z})");
	}

}
