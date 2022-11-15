using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private Vector2 movement;
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask layerMask;


    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3((movement.x * speed * Time.deltaTime) + transform.position.x,
            (movement.y * speed * Time.deltaTime) + transform.position.y, transform.position.z);

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
