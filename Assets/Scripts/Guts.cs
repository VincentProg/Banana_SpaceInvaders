using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guts : MonoBehaviour
{
    
    public float lifeTime;
    public float timeToMoveUp;
    public Vector2 rangeSpeed;
    public Shader desolve;

    [Header("point to stick cam")]
    public List<GameObject> points;


    public Vector3 dir;
    private Collider col;
    private Rigidbody rig;
    private Material cloneMat;
    private float speed;

    private void Awake()
    {
        rig = gameObject.GetComponent<Rigidbody>();
        col = gameObject.GetComponent<Collider>();
        col.isTrigger = true;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timeToMoveUp <= 0)
        {
            rig.velocity.Set(0, 0, 0);
            rig.useGravity = true;
            col.isTrigger = false;
        }
        else
        {
            timeToMoveUp -= Time.deltaTime;
            rig.velocity = dir * speed;
            rig.useGravity = false;

        }

        if(rig.velocity.magnitude == 0)
        { 
            //Debug.Log("start desolve");
            cloneMat.color = new Color(cloneMat.color.r, cloneMat.color.g, cloneMat.color.b, cloneMat.color.a-Time.deltaTime);
            //cloneMat.shader = desolve;
/*            lifeTime -= Time.deltaTime;
            if(lifeTime<=0)
            {
                while(cloneMat.color.a>0)
                {
                    
                }

            }*/
            //Debug.Log(cloneMat.color.a);
        }

        if (cloneMat.color.a <= 0)
        {
            Debug.Log("start desolve");
            Destroy(gameObject);
        }

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
