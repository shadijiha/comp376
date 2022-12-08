using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallUIEffect : MonoBehaviour
{
    [SerializeField]    private Color       m_ActiveColor;
    [SerializeField]    private Color       m_InactiveColor;
    [SerializeField]    private Image       m_HealImage;
    [SerializeField]    private Image       m_AmpImage;
    [SerializeField]    private Image       m_HasteImage;
    [SerializeField]    private AudioSource m_HealAudioSource;
    [SerializeField]    private AudioSource m_AmpAudioSource;
    [SerializeField]    private AudioSource m_HasteAudioSource;

    public void HealActivate()
    {
        m_HealAudioSource.Play();
        m_HealImage.color = m_ActiveColor;
    }

    public void HealDeactivate()
    {
        m_HealAudioSource.Stop();
        m_HealImage.color = m_InactiveColor;
    }

    public void AmpActivate()
    {
        m_AmpAudioSource.Play();
        m_AmpImage.color = m_ActiveColor;
    }

    public void AmpDeactivate()
    {
        m_AmpAudioSource.Stop();
        m_AmpImage.color = m_InactiveColor;
    }

    public void HasteActivate()
    {
        m_HasteAudioSource.Play();
        m_HasteImage.color = m_ActiveColor;
    }

    public void HasteDeactivate()
    {
        m_HasteAudioSource.Stop();
        m_HasteImage.color = m_InactiveColor;
    }
}
