using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector3 movement;
    [SerializeField] private float lifeTime;

    private float _speed;
    private float _timer;

    [SerializeField] private AK.Wwise.Event bulletIdlePlay;
    [SerializeField] private AK.Wwise.Event bulletIdleStop;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void InitBullet(float speed)
    {
        _speed = speed;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, _speed);

        bulletIdlePlay.Post(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position += movement * _speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            Destroy(gameObject);

            bulletIdleStop.Post(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ILivingEntity l_entity = other.transform.GetComponent(typeof(ILivingEntity)) as ILivingEntity;
        
        l_entity?.Death();
        Destroy(gameObject);
        bulletIdleStop.Post(gameObject);
    }

    private void OnCollisionEnter(Collision other)
    {
        Destroy(gameObject);
        bulletIdleStop.Post(gameObject);
    }
}
