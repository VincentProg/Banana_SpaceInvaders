using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootCooldown;

    private float _timer;
    private bool m_isRecoil;
    [SerializeField] private float m_strengthRecoil;
    [SerializeField] private AnimationCurve m_recoilCurve;
    private float m_initialZ;
    
    
    // Start is called before the first frame update
    void Start()
    {
        m_initialZ = transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;

        if (m_isRecoil)
        {
            Recoil();
        }
    }

    public void ShootInput(InputAction.CallbackContext ctx)
    {
        // Shoot once
        if (ctx.performed)
        {
            if (_timer <= 0)
            {
                Bullet shootedBullet = Instantiate(bullet, transform.position, Quaternion.Euler(90,0,0));
                shootedBullet.InitBullet(bulletSpeed);
                _timer += shootCooldown;
                m_isRecoil = true;
            }
        }
    }

    private void Recoil()
    {
        float l_t = (shootCooldown - _timer) / shootCooldown;
        float l_y = m_recoilCurve.Evaluate(l_t);
        float l_Zvalue = Mathf.Lerp(m_initialZ, m_initialZ - m_strengthRecoil, l_y);
        transform.position = new Vector3(transform.position.x, transform.position.y, l_Zvalue);
        if (l_t > 1)
        {
            m_isRecoil = false;
        }
    }
}
