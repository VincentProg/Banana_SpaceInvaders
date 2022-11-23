using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodEffectControler : MonoBehaviour
{
    public float setFPS;
    public float speed;

    private float time = 0;
    private Material cloneMat;
    private Shader cloneShader;
    private int displayFrame = 0;

    private void Awake()
    {
        cloneMat = gameObject.GetComponent<MeshRenderer>().material;
        gameObject.GetComponent<MeshRenderer>().material = cloneMat;
        cloneShader = cloneMat.shader;
        cloneMat.shader = cloneShader;
        cloneMat.SetFloat("_gameTimeAtFirstFrame", 0);
    }


    // Update is called once per frame
    void Update()
    {
        UpdateShader();
        DestroyParent();
    }

    private void UpdateShader()
    {
        time += Time.deltaTime;
        if(time >= (1/setFPS)*speed)
        {
            //Debug.Log("je Marche");
            displayFrame++;
            cloneMat.SetInt("_displayFrame", displayFrame);
            time -= (1 / setFPS)*speed;
        }
    }

    void DestroyParent()
    {
        if (cloneMat.GetInt("_displayFrame") == 45)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
