using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;

public class RythmManager : MonoBehaviour
{
    [HideInInspector]
    public UnityEvent Beat, FourBeats, HeightBeats;
    private int currentNbrBeat;

    [SerializeField] private int m_BPM;
    private float m_delayBeat;

    private bool m_isStarted;
    [SerializeField] private float m_delayBeforeStart;

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
        StartCoroutine(iDelayStart());
    }

    private void Update()
    {
        if (!m_isStarted) return;
        m_currentTime += Time.deltaTime;
        if (m_currentTime > m_delayStep)
        {
            m_currentTime -= m_delayStep;
            CallBeat();
        }
    }

    private IEnumerator iDelayStart()
    {
        yield return new WaitForSeconds(m_delayBeforeStart);
        m_isStarted = true;
    }

    private void CallBeat()
    {
        Debug.Log("Beat " + m_delayBeat + " - Step " + m_delayStep );
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

    public void SetNewRythm(float p_speedRythm)
    {
        m_isNewRythm = true;
        m_speedRythm = p_speedRythm;
    }

    public float GetRemainingTimeBeforeBeat()
    {
        return m_delayStep - m_currentTime;
    }
}
