using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector3 movement;
    [SerializeField] private float lifeTime;
    [SerializeField] private string deathMonster;

    private float _speed;
    private float _timer;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void InitBullet(float speed)
    {
        _speed = speed;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, _speed);
    }

    // Update is called once per frame
    void Update()
    {
       // transform.position += movement * _speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        ILivingEntity l_entity = other.transform.GetComponent(typeof(ILivingEntity)) as ILivingEntity;
        
        l_entity?.Death();
        Destroy(gameObject);
    }
}
