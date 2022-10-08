using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeMenu : MonoBehaviour
{
    public Canvas canvas;
    public Button resumeButton;
    public Button settingsButton;
    public Button quitButton;

    private bool menuEnabled = false;
    private bool playerExist = false;

    // Start is called before the first frame update
    void Start()
    {
        resumeButton.onClick.AddListener(Resume);
        //settingsButton.onClick.AddListener(() => { });
        quitButton.onClick.AddListener(Quit);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {          
            menuEnabled = !menuEnabled;
        }

        // TODO: Find another way, this is very bad
        GameObject[] objects = FindObjectsOfType<GameObject>();
        playerExist = false;

        foreach (var go in objects)
        {

            // If a player instance exists, then we should lock the cursor on exit
            if (go.tag.StartsWith("Player"))
            {
                playerExist = true;
                break;
            }
        }

        if (!menuEnabled)
        {
            if (playerExist)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
        else
            Cursor.lockState = CursorLockMode.None;

        canvas.enabled = menuEnabled;
    }

    void OnDestroy() { 
        resumeButton.onClick.RemoveListener(Resume);
        quitButton.onClick.RemoveListener(Quit);
    }

    void Resume() {
        menuEnabled = false;
    }

    void Quit()
    {
        Application.Quit();
    }
}
