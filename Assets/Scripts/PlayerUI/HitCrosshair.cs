using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitCrosshair : MonoBehaviour
{
    [SerializeField]    private const   int             HIT_ICON_ACTIVE_FRAMES  = 10;
    [SerializeField]    private         int             m_FramesSinceHit        = HIT_ICON_ACTIVE_FRAMES;
    [SerializeField]    private         int             m_hitIndex;
    [SerializeField]    private         Color           m_NormalColor;
    [SerializeField]    private         Color           m_HitColor;
    [SerializeField]    private         Color           m_CritColor;
    [SerializeField]    private         AudioSource     m_AudioSource;
    [SerializeField]    private         AudioClip       m_HitClip;
    [SerializeField]    private         AudioClip       m_CritClip;
    [SerializeField]    private         Image           m_HitCross;
    [SerializeField]    private         bool            m_Crit;
    [SerializeField]    private         bool            m_Hit;

    private void FixedUpdate()
    {
        if (m_Crit)
        {
            m_HitCross.color = m_CritColor;
            m_FramesSinceHit = 0;
            m_AudioSource.PlayOneShot(m_CritClip);

            m_Crit = false;
            m_Hit = false;
        }
        else if (m_Hit)
        {
            m_HitCross.color = m_HitColor;
            m_FramesSinceHit = 0;
            m_AudioSource.PlayOneShot(m_HitClip);

            m_Hit = false;
        }

        if (m_FramesSinceHit < HIT_ICON_ACTIVE_FRAMES)
        {
            ++m_FramesSinceHit;
        }
        else
        {
            m_HitCross.color = m_NormalColor;
        }
    }

    public void Hit()
    {
        m_Hit = true;
    }

    public void Crit()
    {
        m_Crit = true;
    }
}
