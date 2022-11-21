using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyManager : MonoBehaviour
{
    
    [SerializeField]
    private Enemy m_enemyPrefab;

    private List<Enemy> m_enemies = new List<Enemy>();

    [Header("ROWS")]
    [SerializeField][Range(2,50)]
    private int m_nbrColumns;
    [SerializeField][Range(0,50)]
    private float m_offsetX, m_offsetZ;

    [SerializeField] 
    private int m_BPM;

    private float m_delayStep;
    
    private bool m_canStep;

    [Space(20)] [Header("RANDOM SHOOT")]
    [SerializeField]
    private float m_inititalDelayBetweenRandomShoot;
    private float m_currentDelayBetweenRandomShoot;
    [SerializeField] private float m_OffsetDelayRandomShoot;
    private bool m_isRandomShootingEnabled;

    [Header("TARGET SHOOT")] [SerializeField]
    private float m_initialDelayBetweenFocusedShoot;


    // Start is called before the first frame update
    void Start()
    {

        m_canStep = true;
        m_delayStep = 60.0f / m_BPM;
        m_isRandomShootingEnabled = true;
        m_currentDelayBetweenRandomShoot = m_inititalDelayBetweenRandomShoot;

        Referencer.Instance.Player.OffsetMovement = m_offsetX;

    }

    // Update is called once per frame
    void Update()
    {
        RunStepProcess();

        RunShootProcess();
    }

    private void SpawnEnemies()
    {
        int l_randNbrEnemies = Random.Range(1,3);
        int l_randIndex = Random.Range(-1, 1);

        switch (l_randNbrEnemies)
        {
            case 1 :
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

    private Enemy SpawnEnemy(int p_index)
    {
        int l_indexPos =  (p_index + 3) % 3;
        int l_indexPosValue = p_index - 1;
        Enemy l_enemy = Instantiate(m_enemyPrefab,transform);
        l_enemy.transform.position = new Vector3(m_offsetX * l_indexPosValue, transform.position.y, transform.position.z);
        l_enemy.SetIndex(l_indexPos);
        m_enemies.Add(l_enemy);
        return l_enemy;
    }

    #region Step functions

    private void RunStepProcess()
    {
        if (m_canStep)
        {
            m_canStep = false;
            SpawnEnemies();
            StartCoroutine(Step());
        }
    }
    
    private IEnumerator Step()
    {
        foreach (Enemy l_enemy in m_enemies)
        {
            l_enemy.Step(m_offsetX, -m_offsetZ);
        }

        yield return new WaitForSeconds(m_delayStep);

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
        float l_randX = Random.Range(-1, 1) * m_offsetX;
        RaycastHit l_hit;
        Physics.Raycast(new Vector3(l_randX, transform.position.x, -Mathf.Infinity), Vector3.forward, out l_hit, Mathf.Infinity);
        l_hit.transform?.GetComponent<Enemy>()?.Shoot();
        
        float l_delay = Random.Range(
            m_currentDelayBetweenRandomShoot - m_OffsetDelayRandomShoot,
            m_currentDelayBetweenRandomShoot + m_OffsetDelayRandomShoot
        );
        yield return new WaitForSeconds(l_delay);
        m_isRandomShootingEnabled = true;
    }


    #endregion
    
    
}