using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//[RequireComponent(typeof(HealthFunctions))]
public class AmmoPack : NetworkBehaviour
{
    [SerializeField] private int ammoRestorePercent = 50;

    private AudioSource audioSrc;

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }


    float currentAngle = 0;
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(
                                    270,
                                    0,
                                    currentAngle));

        currentAngle += 30 * Time.deltaTime;
        currentAngle = currentAngle % 360;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.LandMineRespawnTime);

        Enabled();
    }

    /// <summary>
    /// Hide from match until respawn
    /// </summary>
    private void Disable()
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    private void Enabled()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains(PlayerShoot.PLAYER_TAG))
        {
            var primary = collision.gameObject.GetComponent<WeaponManager>().mPrimary;

            if (primary.currentSpareAmmo < primary.maxAmmo)
            {
                audioSrc.PlayOneShot(audioSrc.clip);
                primary.currentSpareAmmo = (int)Mathf.Min(
                                        primary.currentSpareAmmo + primary.maxAmmo * ammoRestorePercent / 100.0f,
                                        primary.maxAmmo);


                Disable();
                StartCoroutine(Respawn());
            }
        }

    }
}