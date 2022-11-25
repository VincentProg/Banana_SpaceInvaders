using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour, ILivingEntity
{
    private PlayerInput playerInput;
    private Controls controls;

    [Header("MOUVEMENT")] 
    public float m_movementSpeed;
    private bool m_isMoving;
    private float m_movementX;
    private int m_indexMovement;
    private float m_offsetMovement;
    public int IndexMovement => m_indexMovement;
    [SerializeField] private AnimationCurve m_mouvementCurve;

    [Header("ROTATION")] [SerializeField] private float m_rotationIntensity;
    [SerializeField] private AnimationCurve m_rotationCurve;

    [Header("SHIELD")] [SerializeField] private GameObject m_shield;
    [SerializeField] private float m_disableShieldDelay;
    private Material m_shieldMaterial;

    private float m_currentTimeMovement;
    private float m_initialX, m_targetX;
    [SerializeField] private AK.Wwise.Event dashSound;
    // Start is called before the first frame update
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        controls = new Controls();
        controls.Player.Enable();
        controls.Player.PlayerMovement.performed += PlayerMovementInput;
        m_shieldMaterial = m_shield.GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        m_offsetMovement = Referencer.Instance.EnemyManagerInstance.OffsetX;
        Referencer.Instance.RythmManagerInstance.Beat.AddListener(transform.GetComponent<PlayerShoot>().Shoot);
    }

    private void OnDestroy()
    {
        controls.Player.PlayerMovement.performed -= PlayerMovementInput;
        controls.Player.Disable();
       
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
            dashSound.Post(gameObject);
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

        if (m_movementX == 1)
            Haptics.instance.HapticVRRight(0u, 0.3f,0.1f) ;
        else
            Haptics.instance.HapticVRLeft(0u, 0.3f, 0.1f);
    }

    private void Move()
    {
        m_currentTimeMovement += Time.deltaTime * m_movementSpeed;

        float l_yMouvement = m_mouvementCurve.Evaluate(m_currentTimeMovement);
        float l_xValue = Mathf.Lerp(m_initialX, m_targetX, l_yMouvement);
        transform.position = new Vector3(l_xValue, transform.position.y, transform.position.z);

        if (EffectManager.Instance.IsEffectActive("PlayerFX"))
        {
            float l_yRotation = m_rotationCurve.Evaluate(m_currentTimeMovement);
            transform.rotation = Quaternion.Euler(0, 0, l_yRotation * m_rotationIntensity * -m_movementX);
        }

        if (m_currentTimeMovement >= 1)
        {
            m_isMoving = false;
        }
    }

    public void Death()
    {
        Debug.Log("Death");
        Referencer.Instance.RythmManagerInstance.Beat.RemoveListener(transform.GetComponent<PlayerShoot>().Shoot);
        PlayerDeathManager.Instance.Death();
        Destroy(gameObject);
    }

    public void RemoveShield()
    {
        StartCoroutine(FadeOverTime());
    }

    IEnumerator FadeOverTime()
    {
        for (float t = 0f; t < m_disableShieldDelay; t += Time.deltaTime) {

            if (EffectManager.Instance.IsEffectActive("PlayerFX"))
            {
                float normalizedTime = t / m_disableShieldDelay;
                m_shieldMaterial.SetFloat("_dissolve_amount", Mathf.Lerp(1f, 0f, normalizedTime));
            }

            yield return null;
        }
        m_shieldMaterial.SetFloat("_dissolve_amount", 0f);
        
        m_shield.SetActive(false);
    }
}