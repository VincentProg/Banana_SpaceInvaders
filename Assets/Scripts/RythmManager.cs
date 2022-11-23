using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class RythmManager : MonoBehaviour
{
    public UnityEvent Beat;
    public UnityEvent FourBeats;
    public UnityEvent HeightBeats;
    private int currentNbrBeat;

    [SerializeField] private int m_BPM;
    private float m_delayBeat;

    public float m_delayStep
    {
        get
        {
            return m_delayBeat / m_speedRythm;
        }
    }
    private float m_currentTime;

    [SerializeField] [Range(0, 1)]
    private float m_speedRythm;
    private bool m_isNewRythm;

    private void Start()
    {
        m_delayBeat = 60.0f / m_BPM;
    }

    private void Update()
    {
        m_currentTime += Time.deltaTime * m_speedRythm;
        if (m_currentTime > m_delayStep)
        {
            m_currentTime -= m_delayStep;
            CallBeat();
        }
    }

    private void CallBeat()
    {
        Debug.Log("Beat");
        Beat?.Invoke();
        currentNbrBeat++;
        if (currentNbrBeat % 4 == 0)
        {
            CallFourBeats();
        }

        if (currentNbrBeat % 8 == 0)
        {
            CallHeightBeats();
        }
    }
    
    private void CallFourBeats()
    {
        Debug.Log("FourBeat");
        FourBeats?.Invoke();
    }

    private void CallHeightBeats()
    {
        Debug.Log("HeightBeat");
        HeightBeats?.Invoke();
    }

    private void SetNewRythm(float p_speedRythm)
    {
        m_isNewRythm = true;
        m_speedRythm = p_speedRythm;
    }
}
