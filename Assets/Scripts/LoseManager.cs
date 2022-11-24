using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseManager : MonoBehaviour
{
    public int numberEnemyPastForLose;
    public string loseScene;

    private EnemyManager enemyManager;
    private int count = 0;
    public string[] musics;



    // Start is called before the first frame update
    void Start()
    {
        enemyManager = GameObject.Find("EnemyManager").GetComponent<EnemyManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemyPosition();
    }

    void Lose()
    {
        if(count==numberEnemyPastForLose)
        {
            /*foreach (string music in musics)
            {
                AkSoundEngine.PostEvent(music, this.gameObject);
            }*/

            SceneManager.LoadScene(loseScene) ;
        }
    }

    void CheckEnemyPosition()
    {
        //Debug.Log(enemyManager.GetEnemys().Count);
        foreach(Enemy enemy in enemyManager.GetEnemys())
        {   
            if(enemy.transform.position.z<gameObject.transform.position.z)
            {
                count++;
                Lose();
                Destroy(enemy, 3);
            }
        }
    }
}
