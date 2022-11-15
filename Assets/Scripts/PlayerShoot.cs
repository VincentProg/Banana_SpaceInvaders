using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShoot : MonoBehaviour
{

    [SerializeField] private Bullet bullet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShootInput(InputAction.CallbackContext ctx)
    {
        // Shoot once
        if (ctx.performed)
        {
            Instantiate(bullet, transform.position, Quaternion.identity);
        }
    }
}
