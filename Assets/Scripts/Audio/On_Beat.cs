using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class On_Beat : MonoBehaviour
{
    [SerializeField] private UnityEvent myEvent;
    [SerializeField] private float durationBetweenAnim = 1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitBeat());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator WaitBeat()
    {
        yield return new WaitForSeconds(durationBetweenAnim);
        myEvent.Invoke();
        StartCoroutine(WaitBeat());
    }
}
