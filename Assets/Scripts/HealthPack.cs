using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class HealthPack : NetworkBehaviour
{
    [SerializeField] private uint healing = 20;

    float currentAngle = 0;
    void Update() {
        transform.rotation = Quaternion.Euler(new Vector3(
                                    90,
                                    0 ,
                                    currentAngle));

        currentAngle += 30 * Time.deltaTime;
        currentAngle = currentAngle % 360;
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.HealthPackRespawnTime);

        Enabled();
    }

    /// <summary>
    /// Hide from match until respawn
    /// </summary>
    private void Disable() { 
        GetComponent<Collider>().enabled = false;
        GetComponent<Renderer>().enabled = false;
    }

    private void Enabled() {
        GetComponent<Collider>().enabled = true;
        GetComponent<Renderer>().enabled = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains(PlayerShoot.PLAYER_TAG))
        {
            var playerScript = collision.gameObject.GetComponent<Player>();

            if (playerScript.GetHealth() < playerScript.GetMaxHealth())
            {
                playerScript.HealBy(healing);

                Disable();
                StartCoroutine(Respawn());
            }
        }
    }
}
