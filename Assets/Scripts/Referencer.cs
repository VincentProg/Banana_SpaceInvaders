using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referencer : MonoBehaviour
{
    public static Referencer Instance;

    private Transform m_player;

    public Transform Player
    {
        get
        {
            if (m_player == null)
            {
                m_player = FindObjectOfType<PlayerMovement>().transform;
            }
            return m_player;
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
