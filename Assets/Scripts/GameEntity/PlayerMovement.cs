using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    private PlayerInput playerInput;
    private Controls controls;

    [Header("MOUVEMENT")] [SerializeField] private float m_movementSpeed;
    private bool m_isMoving;
    private float m_movementX;
    [SerializeField] private float m_offsetMovement;
    [SerializeField] private AnimationCurve m_mouvementCurve;

    [Header("ROTATION")] [SerializeField] private float m_rotationIntensity;
    [SerializeField] private AnimationCurve m_rotationCurve;

    private float m_currentTimeMovement;
    private float m_initialX, m_targetX;

    // Start is called before the first frame update
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.PlayerMovement.performed += PlayerMovementInput;
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isMoving)
        {
            Move();
        }
    }

    public void PlayerMovementInput(InputAction.CallbackContext p_ctx)
    {
        int value = (int)p_ctx.ReadValue<float>();
        if (value != 0)
        {
            m_currentTimeMovement = 0;
            m_movementX = value;
            m_initialX = transform.position.x;
            m_targetX = m_initialX + m_offsetMovement * m_movementX;
            m_isMoving = true;
        }
    }
    
    private void Move()
    {
        m_currentTimeMovement += Time.deltaTime * m_movementSpeed;

        float l_yMouvement = m_mouvementCurve.Evaluate(m_currentTimeMovement);
        float l_xValue = Mathf.Lerp(m_initialX, m_targetX, l_yMouvement);
        transform.position = new Vector3(l_xValue, transform.position.y, transform.position.z);

        float l_yRotation = m_rotationCurve.Evaluate(m_currentTimeMovement);
        transform.rotation = Quaternion.Euler(0, 0, l_yRotation * m_rotationIntensity * -m_movementX);

        if (m_currentTimeMovement >= 1)
        {
            m_isMoving = false;
        }
    }
}