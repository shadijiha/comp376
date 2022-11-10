using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuGUI : MonoBehaviour
{
    [SerializeField] private GameObject escapeMenuPanel;

    private bool menuEnabled;
    private PlayerControler playerController;

    // Start is called before the first frame update
    void Start()
    {
        menuEnabled = false;
        escapeMenuPanel.SetActive(menuEnabled);

        Player localPlayer = GetComponentInParent<PlayerUISetup>().localPlayer;
        playerController = localPlayer.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
            menuEnabled = !menuEnabled;

        Cursor.lockState = menuEnabled ? CursorLockMode.None : CursorLockMode.Locked;

        playerController.EnableMovement(!menuEnabled);
        escapeMenuPanel.SetActive(menuEnabled);
    }

    public void Resume()
    {
        menuEnabled = false;
    }

    public void Quit()
    {
        Application.Quit();
    }
}
