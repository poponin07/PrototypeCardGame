//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.2
//     from Assets/Scripts/Input/InputSystem_i.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @InputSystem_i : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @InputSystem_i()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""InputSystem_i"",
    ""maps"": [
        {
            ""name"": ""Camera"",
            ""id"": ""12f8986b-2b3d-45d6-adb4-08e7a1e2f205"",
            ""actions"": [
                {
                    ""name"": ""CursorPosition"",
                    ""type"": ""Value"",
                    ""id"": ""481473b7-82e7-441f-9f99-f643f881881a"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ClickForRayCast"",
                    ""type"": ""Button"",
                    ""id"": ""af41de2a-6a9d-41f3-97f2-2cc079e5c30d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ff8ebdaf-49e1-469b-b47b-8b98be6b32e9"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClickForRayCast"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Camera
        m_Camera = asset.FindActionMap("Camera", throwIfNotFound: true);
        m_Camera_CursorPosition = m_Camera.FindAction("CursorPosition", throwIfNotFound: true);
        m_Camera_ClickForRayCast = m_Camera.FindAction("ClickForRayCast", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Camera
    private readonly InputActionMap m_Camera;
    private ICameraActions m_CameraActionsCallbackInterface;
    private readonly InputAction m_Camera_CursorPosition;
    private readonly InputAction m_Camera_ClickForRayCast;
    public struct CameraActions
    {
        private @InputSystem_i m_Wrapper;
        public CameraActions(@InputSystem_i wrapper) { m_Wrapper = wrapper; }
        public InputAction @CursorPosition => m_Wrapper.m_Camera_CursorPosition;
        public InputAction @ClickForRayCast => m_Wrapper.m_Camera_ClickForRayCast;
        public InputActionMap Get() { return m_Wrapper.m_Camera; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CameraActions set) { return set.Get(); }
        public void SetCallbacks(ICameraActions instance)
        {
            if (m_Wrapper.m_CameraActionsCallbackInterface != null)
            {
                @CursorPosition.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnCursorPosition;
                @CursorPosition.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnCursorPosition;
                @ClickForRayCast.started -= m_Wrapper.m_CameraActionsCallbackInterface.OnClickForRayCast;
                @ClickForRayCast.performed -= m_Wrapper.m_CameraActionsCallbackInterface.OnClickForRayCast;
                @ClickForRayCast.canceled -= m_Wrapper.m_CameraActionsCallbackInterface.OnClickForRayCast;
            }
            m_Wrapper.m_CameraActionsCallbackInterface = instance;
            if (instance != null)
            {
                @CursorPosition.started += instance.OnCursorPosition;
                @CursorPosition.performed += instance.OnCursorPosition;
                @CursorPosition.canceled += instance.OnCursorPosition;
                @ClickForRayCast.started += instance.OnClickForRayCast;
                @ClickForRayCast.performed += instance.OnClickForRayCast;
                @ClickForRayCast.canceled += instance.OnClickForRayCast;
            }
        }
    }
    public CameraActions @Camera => new CameraActions(this);
    public interface ICameraActions
    {
        void OnCursorPosition(InputAction.CallbackContext context);
        void OnClickForRayCast(InputAction.CallbackContext context);
    }
}
