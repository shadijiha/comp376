using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class SensitivitySettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMProText textNumber;

    private PlayerControler playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GetComponentInParent<PlayerUISetup>().playerController;
    }

    // Update is called once per frame
    void Update()
    {
        playerController.sensitivity = slider.value;
        textNumber.text = string.Format("{0:0.00}", slider.value);
    }
}
