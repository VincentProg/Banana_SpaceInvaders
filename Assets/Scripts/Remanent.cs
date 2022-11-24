using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remanent : MonoBehaviour
{
    public float activeTime;

    [Header("Mesh related")]
    public float meshRefreshRate;
    public float meshDestroyDelay;
    public Transform positionToSpawn;

    [Header("Mesh related")]
    public Material mat;
    public string sharderVarRef;
    public float shaderVarRate;
    public float shaderVarRefreshRate;

    private MeshRenderer MeshRenderers;
    private Transform positionParent;
    private bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        positionParent = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(!active)
            StartCoroutine(ActiveTrail(activeTime));
    }

    IEnumerator ActiveTrail(float activeTime)
    {
        active = true;
        while(activeTime>0)
        { 
            activeTime -= meshRefreshRate;

            if (MeshRenderers == null)
                MeshRenderers = GetComponentInChildren<MeshRenderer>();

            GameObject gObj = new GameObject();

            gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
            gObj.transform.localScale = gameObject.transform.localScale;
            MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
            MeshFilter mf = gObj.AddComponent<MeshFilter>();

            gObj.GetComponent<MeshRenderer>().material = mat;
            gObj.GetComponent<MeshFilter>().mesh = gameObject.GetComponent<MeshFilter>().mesh;
            
            Mesh mesh = new Mesh();
            mf.mesh = gameObject.GetComponent<MeshFilter>().mesh;
            mr.material = mat;

            //mf.mesh = mesh;

            //StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

            Destroy(gObj, meshDestroyDelay);
            
            yield return new WaitForSeconds(meshRefreshRate);
        }
        active = false;
    }

    IEnumerator AnimateMaterialFloat(Material mat,float goal,float rate, float refreshRate)
    {
        float valueToAnimte = mat.GetFloat(sharderVarRef);
        while(valueToAnimte>goal)
        {
            valueToAnimte -= rate;
            mat.SetFloat(sharderVarRef, valueToAnimte);
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
