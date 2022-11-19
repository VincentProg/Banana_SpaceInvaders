using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Guts : MonoBehaviour
{
    
    public float lifeTime;
    private float currentTimeCurve;
    public float durationCurve;
    public Vector2 rangeSpeed;
    public Shader dissolve;
    public AnimationCurve speedCurve;
    [Header("point to stick cam")]
    public List<GameObject> points;


    private Vector3 dir;
    private Shader cloneDissolve;
    private Collider col;
    private Rigidbody rig;
    private Material cloneMat;
    private float speed;
    private float advanceDissolve;

    private void Awake()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
        cloneDissolve = dissolve;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), 0);
        //dir = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)).normalized;
        RandomDir();
        dir.y = Random.Range(0.45f, 0.90f);
        gameObject.transform.eulerAngles = new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));

        cloneMat = gameObject.GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<MeshRenderer>().material = cloneMat;
        speed = Random.Range(rangeSpeed.x, rangeSpeed.y);

        rig.velocity = dir * speed ;


    }

    // Update is called once per frame
    void Update()
    {

        currentTimeCurve += Time.deltaTime/durationCurve;
        if (currentTimeCurve <= 1)
        {
            //Debug.Log(dir * (speed * speedCurve.Evaluate(1 - timeToMoveUp)));
            float l_speedY = (speed * speedCurve.Evaluate(currentTimeCurve));
            rig.velocity = rig.velocity.normalized * l_speedY;
            //rig.useGravity = false;
        }

        if(rig.velocity.magnitude == 0)
        {
            if(cloneMat.shader != cloneDissolve)
            {
                cloneMat.shader = cloneDissolve;
            }
            advanceDissolve += Time.deltaTime;
            cloneMat.SetFloat("_advanceDisolve", advanceDissolve);            
        }

        if (advanceDissolve >= 1)
        {
            Debug.Log("start desolve");
            Destroy(gameObject);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        col.isTrigger = false;
    }

    public void RandomDir()
    {
        float radius = 3f;
        Vector3 randomPos = Random.insideUnitSphere * radius;
        randomPos += transform.position;
        randomPos.y = 0;

        dir = randomPos - transform.position;
        dir.Normalize();

        float dotProduct = Vector3.Dot(transform.forward, dir);
        float dotProductAngle = Mathf.Acos(dotProduct / transform.forward.magnitude * dir.magnitude);

        randomPos.x = Mathf.Cos(dotProductAngle) * radius + transform.position.x;
        randomPos.z = Mathf.Sin(dotProductAngle * (Random.value > 0.5f ? 1f : -1f)) * radius + transform.position.z;
        dir = randomPos.normalized;

    }

}
