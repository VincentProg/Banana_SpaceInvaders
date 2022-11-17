using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private float movementX;
    [SerializeField] private float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (movementSpeed * movementX * Time.deltaTime), transform.position.y, transform.position.z);
    }

    public void PlayerMovementInput(InputAction.CallbackContext ctx)
    {
        movementX = ctx.ReadValue<float>();
    }
}
