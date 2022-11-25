using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectScriptDisabler : MonoBehaviour
{
    [SerializeField] private MonoBehaviour m_script;
    [SerializeField] private string m_fxName;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_script.enabled = EffectManager.Instance.IsEffectActive(m_fxName);
    }
}
