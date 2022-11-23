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

    [Header("MOUVEMENT")] 
    public float m_movementSpeed;
    private bool m_isMoving;
    private float m_movementX;
    private int m_indexMovement;
    private float m_offsetMovement;
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

    private void Start()
    {
        m_offsetMovement = Referencer.Instance.EnemyManagerInstance.OffsetX;
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
        int l_value = (int)p_ctx.ReadValue<float>();
        if (l_value != 0)
        {
            m_movementX = l_value;
            m_indexMovement += l_value;
            if (Mathf.Abs(m_indexMovement) > 2)
            {
                m_indexMovement = m_indexMovement > 0 ? 2 : -2;
            }
            m_currentTimeMovement = 0;
            m_initialX = m_targetX;
            m_targetX = m_offsetMovement * m_indexMovement;
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