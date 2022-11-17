using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("MOUVEMENT")]
    [SerializeField] private AnimationCurve m_curveMouvement;
    private bool m_isMoving;

    [SerializeField]
    private float m_initialDurationMovement;

    public float m_InitialDurationMovement => m_initialDurationMovement;
    
    private float m_currentDurationMovement;
    public float m_DurationMovement
    {
        get => m_currentDurationMovement;
        set => m_currentDurationMovement = value;
    }
    
    private float m_currentTime;
    private Vector3 m_initialPos;
    private Vector3 m_targetPos;

    [Header("SHOOT")] [SerializeField] 
    private GameObject m_bullet;

    private bool m_canShoot;
    [SerializeField] private float m_delayBetweenShoot;

    private void Start()
    {
        m_canShoot = true;
        m_currentDurationMovement = m_initialDurationMovement;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoving)
        {
            Move();
        }
    }

    #region Mouvement functions
    
    public void Step(float offsetStepX, float offsetStepY)
    {
        m_initialPos = transform.position;
        m_targetPos = transform.position + new Vector3(offsetStepX, offsetStepY);
        m_isMoving = true;
        m_currentTime = 0;
    }

    public void Move()
    {
        m_currentTime += Time.deltaTime / m_currentDurationMovement;
        float y = m_curveMouvement.Evaluate(m_currentTime);
        transform.position = Vector3.Lerp(m_initialPos, m_targetPos, y);
        if (m_currentTime >= 1)
        {
            m_isMoving = false;
        }
    }

    #endregion
    
    
    public void Shoot()
    {
        m_canShoot = false;
        StartCoroutine(iShoot());
    }

    public IEnumerator iShoot()
    {
        Instantiate(m_bullet, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(m_delayBetweenShoot);
        m_canShoot = true;
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }
    
    
}