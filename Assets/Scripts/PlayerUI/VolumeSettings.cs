using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private TMProText textNumber;
    
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.FindGameObjectWithTag("Soundtrack").GetComponent<AudioSource>();
        slider.value = audioSource.volume * 100;
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = slider.value / 100;
        textNumber.text = slider.value.ToString();
    }
}
