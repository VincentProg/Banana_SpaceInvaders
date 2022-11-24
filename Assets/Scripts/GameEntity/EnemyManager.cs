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

    private int m_delaySpawnFastEnemy;

    private List<Enemy> m_enemies = new List<Enemy>();

    [SerializeField] [Range(0, 50)] private float m_offsetX, m_offsetZ;
    public float OffsetX => m_offsetX;
    [SerializeField] private float m_startPosY;

    private bool m_canStep;
    private bool m_isRandomShootingEnabled;
    private bool m_isProcessDeactivated;


    // Start is called before the first frame update
    void Start()
    {
        m_isRandomShootingEnabled = true;
        // Referencer.Instance.PlayerInstance.OffsetMovement = m_offsetX;
        Referencer.Instance.RythmManagerInstance.Beat.AddListener(Step);
        Referencer.Instance.RythmManagerInstance.FourBeats.AddListener(SpawnEnemies);
        Referencer.Instance.RythmManagerInstance.HeightBeats.AddListener(SpawnFastEnemy);
        Referencer.Instance.RythmManagerInstance.Beat.AddListener(RandomShoot);
        

    }

    // Update is called once per frame
    void Update()
    {
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
    private void SpawnEnemy(int p_index)
    {
        int l_indexPos = (p_index + 3) % 3;
        int l_indexPosValue = l_indexPos - 1;
        Enemy l_enemy = Instantiate(m_enemyPrefab, transform);
        l_enemy.transform.position =
            new Vector3(m_offsetX * l_indexPosValue, m_startPosY, transform.position.z);
        l_enemy.SetIndex(l_indexPos);
        l_enemy.DeathEvent.AddListener(RemoveEnemyFromList);
        m_enemies.Add(l_enemy);
    }

    private void SpawnFastEnemy()
    {
        int l_indexPosValue = Random.Range(0, 2);
        Enemy l_enemy = Instantiate(m_fastEnemyPrefab, transform);
        l_enemy.transform.position =
            new Vector3(m_offsetX * 2 * (l_indexPosValue > 0 ? 1 : -1), m_startPosY, transform.position.z);
        l_enemy.DeathEvent.AddListener(RemoveEnemyFromList);
        m_enemies.Add(l_enemy);
    }

    public List<Enemy> GetEnemys()
    {
        return m_enemies;
    }


    #region Step functions
    
    private void Step()
    {
        foreach (Enemy l_enemy in m_enemies)
        {
            if (l_enemy.IsActivated)
            {
                l_enemy.Step(m_offsetX, -m_offsetZ);
            }
        }
    }

    #endregion


    #region Shoot functions

    private void RunShootProcess()
    {
        // if (m_isRandomShootingEnabled)
        // {
        //     m_isRandomShootingEnabled = false;
        //     StartCoroutine(RandomShoot());
        // }
    }

    private void RandomShoot()
    {
        print("hellooo");
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

        m_isRandomShootingEnabled = true;
    }

    private void RemoveEnemyFromList(Enemy p_enemy)
    {
        m_enemies.Remove(p_enemy);
    }

    #endregion
}