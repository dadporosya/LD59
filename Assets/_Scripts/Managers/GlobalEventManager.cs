using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalEventManager : MonoBehaviour
{

    [SerializeField] private InputActionReference interactAction;
    public List<string> actionLabels=new List<string>();
    public List<InputActionReference> actionReferences = new List<InputActionReference>();
    public Dictionary<string, InputActionReference> actionContainer = new Dictionary<string, InputActionReference>()
    {

    };
    public event Action OnInteractPressed;

    private void OnEnable()
    {
        interactAction.action.performed += HandleInteract;
        interactAction.action.Enable();
    }

    private void OnDisable()
    {
        interactAction.action.performed -= HandleInteract;
        interactAction.action.Disable();
    }

    private void HandleInteract(InputAction.CallbackContext ctx)
    {
        OnInteractPressed?.Invoke();
    }

    private void Awake()
    {
        for (int i = 0; i < actionLabels.Count; i++)
        {
            actionContainer.Add(actionLabels[i], actionReferences[i]);
        }
    }
}