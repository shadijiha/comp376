using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class WeaponButton : MonoBehaviour, IPointerEnterHandler
{
    [SerializeField] private bool isSelected;
    [SerializeField] private Image image;

    [SerializeField] private TMProText nameText;

    [Header("Text color")]
    [SerializeField] private Color textColorWhenSelected = Color.black;
    [SerializeField] private Color textColorWhenUnselected = Color.white;

    [Header("Text color")]
    [SerializeField] private Color imageColorWhenSelected = Color.white;
    [SerializeField] private Color imageColorWhenUnselected = Color.clear;

    public WeaponLoadout weaponLoadout;
    public PlayerWeapon playerWeapon;
    public bool isPrimaryWeapon;

    // Start is called before the first frame update
    void Start()
    {
        // Set name and description
        nameText.text = playerWeapon.name;

    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            nameText.color = textColorWhenSelected;
            image.color = imageColorWhenSelected;
        }
    }

    public void SelectWeapon()
    {
        isSelected = true;
        weaponLoadout.SelectWeapon(this);
    }

    public void ResetSelection()
    {
        isSelected = false;

        nameText.color = textColorWhenUnselected;
        image.color = imageColorWhenUnselected;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        weaponLoadout.ShowPreview(playerWeapon);
    }
}
