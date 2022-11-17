using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    
    [SerializeField]
    private Transform m_enemyPrefab;

    private Enemy[,] m_enemies;

    [Header("ROWS")] 
    [SerializeField] [Range(3, 10)]
    private int m_nbrLines;
    [SerializeField][Range(5,50)]
    private int m_nbrColumns;
    [SerializeField][Range(0,5)]
    private float m_offsetX, m_offsetY;

    [Space(20)][Header("STEP")]
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
    private bool m_canAccelerate;
    [SerializeField] private float m_delayBetweenAccelerations;
    private float m_timerAcceleration;
    [SerializeField] [Range(0.0001f,1)]
    private float m_minDelayValue;
    [SerializeField] 
    private float m_stepAcceleration;
    private float m_currentDelayStep;
    private bool m_canStep;

    [Space(20)] [Header("RANDOM SHOOT")]
    [SerializeField]
    private float m_inititalDelayBetweenRandomShoot;
    private float m_currentDelayBetweenRandomShoot;
    [SerializeField] private float m_OffsetDelayRandomShoot;
    private bool m_isRandomShootingEnabled;

    [Header("TARGET SHOOT")] [SerializeField]
    private float m_initialDelayBetweenFocusedShoot;
    private float m_currentDelayBetweenFocusedShoot;
    [SerializeField] private float m_offsetDelayTargetShoot;
    private bool m_isTargetShootingEnabled;


    // Start is called before the first frame update
    void Start()
    {
        InitializeWave();

        m_canStep = true;
        m_currentDelayStep = m_initialDelayBetweenStep;
        m_currentDelayBetweenRow = m_initialDelayBetweenRow;
        
        m_isRandomShootingEnabled = true;
        m_currentDelayBetweenRandomShoot = m_inititalDelayBetweenRandomShoot;
        m_isTargetShootingEnabled = true;
        m_currentDelayBetweenFocusedShoot = m_initialDelayBetweenFocusedShoot;
    }

    // Update is called once per frame
    void Update()
    {
        RunStepProcess();
        
        RunAccelerationProcess();

        RunShootProcess();
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
                l_enemy.transform.position = new Vector3(
                    j * m_offsetX - l_initialOffsetX,
                    i * m_offsetY - l_initialOffsetY + transform.position.y,
                    0);
                m_enemies[i, j] = l_enemy;
            }
        }
    }

    #region Step functions

    private void RunStepProcess()
    {
        if (m_canStep)
        {
            m_canStep = false;
            StartCoroutine(Step());
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

        yield return new WaitForSeconds(m_currentDelayStep);
        if (m_canAccelerate)
        {
            m_canAccelerate = false;
            AccelerateStep();
        }

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


    private void RunAccelerationProcess()
    {
        if (!m_canAccelerate)
        {
            m_timerAcceleration += Time.deltaTime;
            if (m_timerAcceleration > m_delayBetweenAccelerations)
            {
                m_canAccelerate = true;
                m_timerAcceleration = 0;
            }
        }
    }

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
    #endregion


    #region Shoot functions

    private void RunShootProcess()
    {
        if (m_isRandomShootingEnabled)
        {
            m_isRandomShootingEnabled = false;
            StartCoroutine(RandomShoot());
        }

        if (m_isTargetShootingEnabled)
        {
            m_isTargetShootingEnabled = false;
            StartCoroutine(TargetShoot());
        }
    }

    private IEnumerator RandomShoot()
    {
        GetRandomShooter().Shoot();
        float l_delay = Random.Range(
            m_currentDelayBetweenRandomShoot - m_OffsetDelayRandomShoot,
            m_currentDelayBetweenRandomShoot + m_OffsetDelayRandomShoot
        );
        yield return new WaitForSeconds(l_delay);
        m_isRandomShootingEnabled = true;
    }

    private IEnumerator TargetShoot()
    {
        GetClosestEnemy()?.Shoot();
        float l_delay = Random.Range(
            m_currentDelayBetweenFocusedShoot - m_offsetDelayTargetShoot,
            m_currentDelayBetweenFocusedShoot + m_offsetDelayTargetShoot
        );
        yield return new WaitForSeconds(l_delay);
        m_isTargetShootingEnabled = true;
    }

    private Enemy GetFirstEnemyAliveInColumn(int p_column)
    {
        for (int j = 0; j < m_nbrLines; j++)
        {
            if (m_enemies[j, p_column].gameObject.activeSelf)
            {
                return m_enemies[j,p_column];
            }
        }

        return null;
    }

    private Enemy GetRandomShooter()
    {
        int l_random = Random.Range(0, m_nbrColumns);
        Enemy l_shooter = null;
        // Get random column to find shooter. If no enemy's alive -> try next column
        for (int i = 0; i < m_nbrColumns; i++)
        {
            int l_column = (l_random + i) % m_nbrColumns;
            l_shooter = GetFirstEnemyAliveInColumn(l_column);
            if (l_shooter)
            {
                break;
            }
        }
        return l_shooter;
    }

    private Enemy GetClosestEnemy()
    {
        Vector3 l_playerPos = Referencer.Instance.Player.position;
        float l_shortestDistance = Mathf.Infinity;
        int m_indexClosestColumn = 0;
        for (int j = 0; j < m_nbrColumns; j++)
        {
            float l_distance = (m_enemies[0, j].transform.position - l_playerPos).sqrMagnitude;
            if (l_distance < l_shortestDistance)
            {
                l_shortestDistance = l_distance;
                m_indexClosestColumn = j;
                
            }
        }
        return GetFirstEnemyAliveInColumn(m_indexClosestColumn);
    }

    #endregion
    
    
}