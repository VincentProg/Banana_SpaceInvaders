using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Launch_MusicMainMenu : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event musicPlay;
    [SerializeField] private AK.Wwise.Event musicStop;

    [SerializeField] private AK.Wwise.Event selectSound;
    [SerializeField] private AK.Wwise.Event validateSound;

    // Start is called before the first frame update
    void Start()
    {
        musicPlay.Post(gameObject);
    }

    private void OnDisable()
    {
        musicStop.Post(gameObject);
    }
    // Update is called once per frame
    void Update()
    {

            

        
    }

    public void SoundOnValidate()
    {
        validateSound.Post(gameObject);
    }

    public void SoundOnSelect()
    {
        selectSound.Post(gameObject);
    }
}
