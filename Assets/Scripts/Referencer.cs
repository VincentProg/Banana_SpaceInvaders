using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Referencer : MonoBehaviour
{
    public static Referencer Instance;
    private PlayerMovement m_player;
    private EnemyManager m_enemymanager;
    private RythmManager m_rythmManager;
    private Scoring m_scoring;
    public PlayerMovement PlayerInstance
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

    public EnemyManager EnemyManagerInstance
    {
        get
        {
            if (m_enemymanager == null)
            {
                m_enemymanager = FindObjectOfType<EnemyManager>();
            }
            return m_enemymanager;
        }
    }

    public RythmManager RythmManagerInstance
    {
        get
        {
            if (m_rythmManager == null)
            {
                m_rythmManager = FindObjectOfType<RythmManager>();
            }
            return m_rythmManager;
        }
    }

    public Scoring ScoringInstance
    {
        get
        {
            if (m_scoring == null)
            {
                m_scoring = FindObjectOfType<Scoring>();
            }
            return m_scoring;
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
