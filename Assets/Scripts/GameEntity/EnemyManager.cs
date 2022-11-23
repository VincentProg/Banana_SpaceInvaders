using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Events;


public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy m_enemyPrefab;
    [SerializeField] private Enemy m_fastEnemyPrefab;
    [SerializeField] private int m_delayFirstSpawnFast;
    [SerializeField] private int m_minDelaySpawnFast;
    [SerializeField] private int m_maxDelaySpawnFast;
    
    private int m_delaySpawnFastEnemy;

    private List<Enemy> m_enemies = new List<Enemy>();

    [Header("ROWS")] [SerializeField] [Range(2, 50)]
    private int m_nbrColumns;

    [SerializeField] [Range(0, 50)] private float m_offsetX, m_offsetZ;
    [SerializeField] private float m_startPosY;
    [SerializeField] private int m_BPM;

    public float OffsetMovement
    {
        get => m_offsetX;
    }
    
    private float m_delayStep;
    private float m_currentNbrStepBeforeSpawn;

    private bool m_canStep;

    private bool m_isRandomShootingEnabled;

    private bool m_isBassStarted;
    private float m_initialDelay;

    private bool m_isProcessDeactivated;


    // Start is called before the first frame update
    void Start()
    {
        m_isBassStarted = false;
        m_canStep = true;
        m_delayStep = 60.0f / m_BPM;
        m_initialDelay = m_delayStep * 4;
        m_delaySpawnFastEnemy = m_delayFirstSpawnFast;
        m_isRandomShootingEnabled = true;

        // Referencer.Instance.PlayerInstance.OffsetMovement = m_offsetX;

        StartCoroutine(StartBass());
    }

    // Update is called once per frame
    void Update()
    {
        RunStepProcess();

        RunShootProcess();
    }

    private IEnumerator StartBass()
    {
        yield return new WaitForSeconds(12f);
        m_isProcessDeactivated = true;
        yield return new WaitForSeconds(1.4f);
        m_isProcessDeactivated = false;
        m_isBassStarted = true;
    }

    private void SpawnEnemies()
    {
        int l_randNbrEnemies = Random.Range(1, 4);
        int l_randIndex = Random.Range(-1, 1);

        switch (l_randNbrEnemies)
        {
            case 1:
                SpawnEnemy(l_randIndex);
                break;
            case 2:
                SpawnEnemy(l_randIndex + 1);
                SpawnEnemy(l_randIndex + 2);
                break;
            case 3:
                SpawnEnemy(l_randIndex);
                SpawnEnemy(l_randIndex + 1);
                SpawnEnemy(l_randIndex + 2);
                break;
        }
    }

    private void SpawnFastEnemy()
    {
        int l_indexPosValue = Random.Range(0, 2);
        Enemy l_enemy = Instantiate(m_fastEnemyPrefab, transform);
        l_enemy.transform.position =
            new Vector3(m_offsetX * 2 * (l_indexPosValue > 0 ? 1 : -1), m_startPosY, transform.position.z);
        l_enemy.TimeUp = m_delayStep - 0.1f;
        l_enemy.DeathEvent.AddListener(RemoveEnemyFromList);
        m_enemies.Add(l_enemy);
    }

    private void SpawnEnemy(int p_index)
    {
        int l_indexPos = (p_index + 3) % 3;
        int l_indexPosValue = l_indexPos - 1;
        Enemy l_enemy = Instantiate(m_enemyPrefab, transform);
        l_enemy.transform.position =
            new Vector3(m_offsetX * l_indexPosValue, m_startPosY, transform.position.z);
        l_enemy.SetIndex(l_indexPos);
        l_enemy.TimeUp = m_isBassStarted ? m_delayStep - 0.1f : m_initialDelay - 0.2f;
        l_enemy.DeathEvent.AddListener(RemoveEnemyFromList);
        m_enemies.Add(l_enemy);
    }

    #region Step functions

    private void RunStepProcess()
    {
        if (m_isProcessDeactivated) return;
        if (m_canStep)
        {
            m_canStep = false;
            if (m_currentNbrStepBeforeSpawn > 0)
            {
                m_currentNbrStepBeforeSpawn--;
            }
            else
            {
                m_currentNbrStepBeforeSpawn = m_nbrColumns;
                SpawnEnemies();
                if (m_delaySpawnFastEnemy <= 0)
                {
                    SpawnFastEnemy();
                    m_delaySpawnFastEnemy = Random.Range(m_minDelaySpawnFast, m_maxDelaySpawnFast + 1);
                }
                else
                {
                    m_delaySpawnFastEnemy--;
                }
            }
            StartCoroutine(Step((m_isBassStarted) ? m_delayStep : m_initialDelay));
        }
    }

    private IEnumerator Step(float p_delay)
    {
        foreach (Enemy l_enemy in m_enemies)
        {
            if (l_enemy.IsActivated)
            {
                l_enemy.Step(m_offsetX, -m_offsetZ);
            }
        }

        yield return new WaitForSeconds(p_delay);
        m_canStep = true;
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
    }

    private IEnumerator RandomShoot()
    {
        float l_randX = Random.Range(-1, 2) * m_offsetX;
        RaycastHit l_hit;
        if (Physics.Raycast(new Vector3(l_randX, transform.position.y, -50),
                Vector3.forward,
                out l_hit,
                500,
        1 << 6)
                )
        {
            Enemy l_enemy = l_hit.transform.GetComponent<Enemy>();
            if (l_enemy is { IsActivated: true })
            {
                l_enemy.Shoot();
            }
        }
        
        yield return new WaitForSeconds(m_isBassStarted ? m_delayStep : m_initialDelay);
        m_isRandomShootingEnabled = true;
    }

    private void RemoveEnemyFromList(Enemy p_enemy)
    {
        m_enemies.Remove(p_enemy);
    }

    #endregion
}