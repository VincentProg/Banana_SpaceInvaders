using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referencer : MonoBehaviour
{
    public static Referencer Instance;

    private PlayerMovement m_player;
    private EnemyManager m_enemymanager;

    public PlayerMovement Player
    {
        get
        {
            if (m_player == null)
            {
                m_player = FindObjectOfType<PlayerMovement>();
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
