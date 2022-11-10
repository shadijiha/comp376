using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMProText = TMPro.TextMeshProUGUI;

public class ButtonBehavior : EventTrigger
{
    private Image image;
    private TMProText text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<TMProText>();
        image = GetComponent<Image>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        // Show black text and white background when going on button
        SetTextColor(Color.black);
        SetBackgroundColor(Color.white);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        // Show white text and transparent background when leaving button
        SetTextColor(Color.white);
        SetBackgroundColor(Color.clear);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        // Show white text and transparent background when leaving button
        SetTextColor(Color.white);
        SetBackgroundColor(Color.clear);
    }

    private void SetTextColor(Color color)
    {
        text.color = color;
    }

    private void SetBackgroundColor(Color color)
    {
        image.color = color;
    }
}
