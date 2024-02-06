using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager_SINGLE : MonoBehaviour
{
    public static InputManager_SINGLE Instance { get; private set; }

    public event EventHandler OnMouseClickStarted;
    public event EventHandler OnMouseClickCanceled;

    private PlayerInputActions playerInputActions;

    private AnimationCaller lastClickedAnimationCaller = null;
    private Camera mainCamera;
    private bool isClicked = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There are more than one InputManager_SINGLE");
        }
        Instance = this;

        playerInputActions = new PlayerInputActions();
        mainCamera = Camera.main;

        playerInputActions.Mouse.Enable();

        playerInputActions.Mouse.Click.started += Click_started;
        playerInputActions.Mouse.Click.canceled += Click_canceled;
    }

    private void Click_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        Physics.Raycast(ray, out RaycastHit hit);

        if (hit.collider != null && hit.collider.TryGetComponent(out AnimationCaller animationCaller)) 
        {
            lastClickedAnimationCaller = animationCaller;

            animationCaller.PlayAnimation();

            isClicked = true;
        }
    }

    private void Update()
    {
        if (isClicked)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
            Physics.Raycast(ray, out RaycastHit hit);
            if (hit.collider == null || hit.collider.TryGetComponent(out AnimationCaller animationCaller) != lastClickedAnimationCaller)
            {
                lastClickedAnimationCaller.PlayExitAnimation();
                isClicked = false;
            }
            else
            {
                animationCaller.PlayAnimation();
            }
        }
    }

    private void Click_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        isClicked = false;

        if (lastClickedAnimationCaller != null)
        {
            lastClickedAnimationCaller.PlayExitAnimation();
        }
    }

    private void OnDestroy()
    {
        playerInputActions.Mouse.Click.started -= Click_started;
        playerInputActions.Mouse.Click.canceled -= Click_canceled;

        playerInputActions.Dispose();
    }
}
