using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class On_Beat : MonoBehaviour
{
    [SerializeField] private UnityEvent[] myEvent;
    public AK.Wwise.Event getMarkerOnMusic;
    // Start is called before the first frame update
    void Start()
    {
        getMarkerOnMusic.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncUserCue, PrintThis);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    public void PrintThis(object in_cookie, AkCallbackType in_type, object in_info)
    {
        print("Vamos");
        for(int i =0; i < myEvent.Length; i++)
        {
            myEvent[i].Invoke();
        }
    }
}
