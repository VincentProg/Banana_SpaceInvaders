using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeathManager : MonoBehaviour
{
    
    public static PlayerDeathManager Instance;

    [Header("Player Info")]
    [SerializeField] private GameObject PlayerSpaceShip;
    
    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);

        Instance = this;
    }

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Death()
    {
        OnDeath();
    }

    private void OnDeath()
    {
        // Spawn new at spawn point;
        Instantiate(PlayerSpaceShip, transform.position, Quaternion.identity);
        AkSoundEngine.PostEvent("Play_Chara_R_TakeDamage", this.gameObject);
        Referencer.Instance.ScoringInstance.ResetOnDeath();
    }
}
