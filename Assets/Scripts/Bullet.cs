using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Vector2 movement;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float lifeTime;


    private float _speed;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void InitBullet(float speed)
    {
        _speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((movement.x * _speed * Time.deltaTime) + transform.position.x,
            (movement.y * _speed * Time.deltaTime) + transform.position.y, transform.position.z);

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // Death
        if (other.gameObject.layer == layerMask.value)
        {
            Destroy(gameObject);
        }
    }
}
