using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyManager : MonoBehaviour
{

    [SerializeField]
    private Transform m_enemyPrefab;

    private Enemy[,] m_enemies;

    [Header("ROWS")] 
    [SerializeField] [Range(3, 10)]
    private int m_nbrLines;
    [SerializeField][Range(5,15)]
    private int m_nbrColumns;
    [SerializeField][Range(0,5)]
    private float m_offsetX, m_offsetY;

    [Header("STEP")]
    [SerializeField]
    private int m_numberStep;
    private int m_currentStep;
    private bool m_isReversed;
    [SerializeField][Range(0,1)]
    private float m_offsetStepX;
    [SerializeField][Range(-5,0)]
    private float m_offsetStepY;
    [SerializeField] 
    private float m_initialDelayBetweenRow;
    private float m_currentDelayBetweenRow;

    [Header("STEP ACCELERATOR")] [SerializeField] [Range(0,1)]
    private float m_initialDelayBetweenStep;
    [SerializeField] [Range(0.0001f,1)]
    private float m_minDelayValue;
    [SerializeField] 
    private float m_stepAcceleration;
    private float m_currentTimeStep;
    private float m_currentDelayStep;
    private bool m_canStep;


    // Start is called before the first frame update
    void Start()
    {
        InitializeWave();
        m_currentDelayStep = m_initialDelayBetweenStep;
        m_canStep = true;
        m_currentDelayBetweenRow = m_initialDelayBetweenRow;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_canStep)
        {
            m_currentTimeStep += Time.deltaTime;
            if (m_currentTimeStep >= m_currentDelayStep)
            {
                m_canStep = false;
                m_currentTimeStep -= m_currentDelayStep;
                StartCoroutine(Step());
            }
        }
    }

    private void InitializeWave()
    {
        m_enemies = new Enemy[m_nbrLines, m_nbrColumns];

        float l_initialOffsetX = (m_offsetX * m_nbrColumns)/2 - m_offsetX/2 + (m_offsetStepX * m_numberStep)/2; 
        float l_initialOffsetY = (m_offsetY * m_nbrLines)/2 - m_offsetY/2;
        for(int i = 0; i < m_nbrLines; i++)
        {
            for(int j = 0; j < m_nbrColumns; j++)
            {
                GameObject l_enemyObject = (GameObject)PrefabUtility.InstantiatePrefab(m_enemyPrefab.gameObject, transform);
                Enemy l_enemy = l_enemyObject.GetComponent<Enemy>();
                l_enemy.transform.position = new Vector3(j * m_offsetX - l_initialOffsetX, i * m_offsetY - l_initialOffsetY, 0);
                m_enemies[i, j] = l_enemy;
            }
        }
        
    }

    #region Step functions
    private IEnumerator Step()
    {
        int l_incrementor = (m_isReversed) ? -1 : 1;
        m_currentStep += l_incrementor;
        bool l_isClampReached = false;
        if (m_currentStep == 0 || m_currentStep == m_numberStep)
        {
            m_isReversed = !m_isReversed;
            l_isClampReached = true;
        }
        
        if (l_isClampReached)
        {
            for (int i = 0; i < m_nbrLines; i++)
            {
                VerticalStep(i);
                yield return new WaitForSeconds(m_currentDelayBetweenRow);
            }
        } else {
            if (l_incrementor > 0)
            {
                for (int i = m_nbrColumns - 1; i >= 0; i--)
                {
                    HorizontalStep(i, l_incrementor);
                    yield return new WaitForSeconds(m_currentDelayBetweenRow);
                }
            }
            else
            {
                for (int i = 0; i < m_nbrColumns; i++)
                {
                    HorizontalStep(i, l_incrementor);
                    yield return new WaitForSeconds(m_currentDelayBetweenRow);
                }
            }
        }
        AccelerateStep();
        m_currentTimeStep = 0;
        m_canStep = true;
    }

    private void VerticalStep(int line)
    {
        for (int j = 0; j < m_nbrColumns; j++)
        {
            m_enemies[line, j].Step(0, m_offsetStepY);
        }
    }

    private void HorizontalStep(int column, int p_incrementor)
    {
        for (int j = 0; j < m_nbrLines; j++)
        {
            m_enemies[j, column].Step(m_offsetStepX * p_incrementor, 0);
        }
    }
    
    #endregion

    private void AccelerateStep()
    {
        if (m_currentDelayStep > m_minDelayValue)
        {
            m_currentDelayStep -= (m_stepAcceleration) / 100.0f;
            if (m_currentDelayStep < m_minDelayValue)
            {
                m_currentDelayStep = m_minDelayValue;
            }
            
            float l_factor = m_currentDelayStep / m_initialDelayBetweenStep;
            for(int i = 0; i < m_nbrLines; i++)
            {
                for(int j = 0; j < m_nbrColumns; j++)
                {
                    m_enemies[i, j].m_DurationMovement = m_enemies[i,j].m_InitialDurationMovement * l_factor;
                }
            }

            m_currentDelayBetweenRow = m_initialDelayBetweenRow * l_factor;

        }
    }
}