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

    private MeshRenderer[] MeshRenderers;
    private Transform positionParent;
    // Start is called before the first frame update
    void Start()
    {
        positionParent = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ActiveTrail(activeTime));
    }

    IEnumerator ActiveTrail(float activeTime)
    {
        while(activeTime>0)
        {
            activeTime -= meshRefreshRate;

            if (MeshRenderers == null)
                MeshRenderers = GetComponentsInChildren<MeshRenderer>();

            for(int i = 0;i<MeshRenderers.Length;i++)
            {
                GameObject gObj = new GameObject();
                gObj.transform.SetPositionAndRotation(positionToSpawn.position, positionToSpawn.rotation);
               
                MeshRenderer mr = gObj.AddComponent<MeshRenderer>();
                MeshFilter mf = gObj.AddComponent<MeshFilter>();

                Mesh mesh = new Mesh();
                mr.material = mat;

                StartCoroutine(AnimateMaterialFloat(mr.material, 0, shaderVarRate, shaderVarRefreshRate));

                Destroy(gObj, meshDestroyDelay);
            }

            yield return new WaitForSeconds(meshRefreshRate);
        }        
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
