using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private AnimationCurve curveMouvement;
    private bool isMoving;

    [SerializeField] private float durationMovement;
    private float currentTime;

    private Vector3 m_initialPos;
    private Vector3 m_targetPos;

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            Move();
        }
    }

    public void Step(float offsetStepX, float offsetStepY)
    {
        m_initialPos = transform.position;
        m_targetPos = transform.position + new Vector3(offsetStepX, offsetStepY);
        isMoving = true;
        currentTime = 0;
    }

    public void Move()
    {
        currentTime += Time.deltaTime / durationMovement;
        float y = curveMouvement.Evaluate(currentTime);
        transform.position = Vector3.Lerp(m_initialPos, m_targetPos, y);
        if (currentTime >= 1)
        {
            isMoving = false;
        }
        
    }

    public void Death()
    {
        gameObject.SetActive(false);
    }
}
