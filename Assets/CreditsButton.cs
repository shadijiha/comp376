using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject creditsModal;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        creditsModal.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        creditsModal.SetActive(false);
    }
}
