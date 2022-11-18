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
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > 0)
            _timer -= Time.deltaTime;
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
            }
        }
    }
}
