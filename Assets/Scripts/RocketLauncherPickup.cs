using System.Collections;
using UnityEngine;

public class RocketLauncherPickup : MonoBehaviour
{
    [SerializeField] private GameObject renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    float currentAngle = 0;
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(new Vector3(
                                    -25,
                                    currentAngle,
                                    0));

        currentAngle += 30 * Time.deltaTime;
        currentAngle = currentAngle % 360;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains(PlayerShoot.PLAYER_TAG))
        {
            EquipeRocketLauncher(collision.gameObject);
            Disable();
            StartCoroutine(Respawn());
        }
    }

    private void EquipeRocketLauncher(GameObject p) {
        var weaponManager = p.GetComponent<WeaponManager>();
        weaponManager.mSuper = new RocketLauncher();
        weaponManager.Equip(weaponManager.mSuper);
    }

    /// <summary>
    /// Hide from match until respawn
    /// </summary>
    private void Disable()
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        renderer.SetActive(false);
    }

    private void Enabled()
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        renderer.SetActive(true);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.MATCH_SETTINGS.RocketLauncherRespawnTime);

        Enabled();
    }
}
