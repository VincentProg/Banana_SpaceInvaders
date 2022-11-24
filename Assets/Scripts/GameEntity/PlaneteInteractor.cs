using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class PlaneteInteractor : MonoBehaviour
{
    private Controls controls;

    [SerializeField] private float m_RotationSpeed;

    private string[] planete = { "Mercure", "Neptune", "Uranus", "Jupiter" };
    private int index = 0;
    private int rotateValue = 90;
    private float targetRotation;
    private bool needToRotate;
    private int currentRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        controls = new Controls();
        controls.Player.Enable();
        controls.Player.ChangeMenu.performed += OnChangeMenu;
        controls.Player.Validate.performed += OnValidateInput;
    }

    private void OnValidateInput(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            switch (index)
            {
                case 0:
                    SceneManager.LoadScene(1);
                    break;
                case 1:
                    // Display controls
                    break;
                case 2:
                    Application.Quit();
                    break;
                case 3:
                    // Display credits
                    break;
                default:
                    Debug.Log($"Keyboard {index} not handled");
                    break;
            }
        }
    }

    private void OnChangeMenu(InputAction.CallbackContext obj)
    {
        if (obj.performed)
        {
            float value = obj.ReadValue<float>();
            index += Mathf.RoundToInt(value);
            rotateValue *= Mathf.RoundToInt(value);
            targetRotation = Mathf.RoundToInt(value);
            
            if (index >= planete.Length)
            {
                index = 0;
            }
            if (index < 0)
            {
                index = planete.Length - 1;
            }
            
            Debug.Log($"New index {index}");
            needToRotate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (needToRotate)
        {
            transform.Rotate(Vector3.up, -targetRotation);
            currentRotation += 1;
            if (currentRotation == Mathf.Abs(rotateValue))
            {
                transform.rotation = Quaternion.Euler(0f, -index * 90, 0f);
                needToRotate = false;
                currentRotation = 0;
            }
        }
    }

    public void OnSelect(SelectEnterEventArgs args)
    {
        switch (args.interactableObject.transform.gameObject.name)
        {
            case "Mercure":
                SceneManager.LoadScene(1);
                break;
            case "Neptune":
                // Display controls
                break;
            case "Uranus":
                Application.Quit();
                break;
            case "Jupiter":
                // Display credits
                break;
            default:
                Debug.Log($"Interractable {args.interactableObject.transform.gameObject.name} not handled");
                break;
        }
    }
}
