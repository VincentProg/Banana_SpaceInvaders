using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wwise_ScriptTest : MonoBehaviour
{
    [SerializeField] private string strBeepTest;
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent(strBeepTest, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
