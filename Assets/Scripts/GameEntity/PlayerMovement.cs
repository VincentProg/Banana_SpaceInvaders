using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    private Vector2 movement;
    [SerializeField] private float movementSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (movementSpeed * movement.x * Time.deltaTime), transform.position.y, transform.position.z);
    }

    public void PlayerMovementInput(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }
}
