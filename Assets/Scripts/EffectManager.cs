using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    private List<string> effectActivated = new List<string>();

    public void ActivateEffect(string uuid)
    {
        if (!effectActivated.Contains(uuid))
        {
            effectActivated.Add(uuid);
        }
    }

    public void DisableEffect(string uuid)
    {
        if (effectActivated.Contains(uuid))
        {
            effectActivated.Remove(uuid);
        }
    }

    public void ToggleEffect(string uuid)
    {
        if (effectActivated.Contains(uuid))
        {
            effectActivated.Remove(uuid);
        }
        else
        {
            effectActivated.Add(uuid);
        }
    }

    public bool IsEffectActive(string uuid)
    {
        return effectActivated.Contains(uuid);
    }
}