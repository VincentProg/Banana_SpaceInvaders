using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGutsManager : MonoBehaviour
{
    public List<GameObject> guts;
    public int nGust;

    private float time;
    private void Start()
    {
        
    }

    private void Update()
    {
        time += Time.deltaTime;
        if(time >3)
        {
            StartExplosion();
            time = 0;
        }

    }

    public void StartExplosion ()
    {
        for(int i = 0;i<nGust; i++)
        {
            Instantiate<GameObject>(guts[Random.Range(0, guts.Count)],gameObject.transform.position,Quaternion.identity); 
        }
    }
}
