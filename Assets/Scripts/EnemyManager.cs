using System;
using System.Collections;
using System.Collections.Generic;
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
    private float m_timeStep;
    private float m_currentTimeStep;
    [SerializeField]
    private int m_numberStep;
    private int m_currentStep;
    private bool m_isReversed;
    [SerializeField][Range(0,1)]
    private float m_offsetStepX;
    [SerializeField][Range(-5,0)]
    private float m_offsetStepY;
    [SerializeField] 
    private float m_delayBetweenEnemy;

    // Start is called before the first frame update
    void Start()
    {
        InitializeWave();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        m_currentTimeStep += Time.fixedDeltaTime;
        if (m_currentTimeStep >= m_timeStep)
        {
            m_currentTimeStep -= m_timeStep;
            StartCoroutine(Step());
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
        
        for (int i = 0; i < m_nbrLines; i++)
        {
            for (int j = 0; j < m_nbrColumns; j++)
            {
                m_enemies[i,j].Step(
                    (l_isClampReached) ? 0 : m_offsetStepX * l_incrementor,
                    (l_isClampReached) ? m_offsetStepY : 0
                    );
            }
            yield return new WaitForSeconds(m_delayBetweenEnemy);
        }
    }
}