//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.4.4
//     from Assets/Configurations/Navigation.inputactions
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

namespace Asteroids.UI.Input
{
    public partial class @NavigationInputCollection : IInputActionCollection2, IDisposable
    {
        public InputActionAsset asset { get; }
        public @NavigationInputCollection()
        {
            asset = InputActionAsset.FromJson(@"{
    ""name"": ""Navigation"",
    ""maps"": [
        {
            ""name"": ""Navigation"",
            ""id"": ""466aebfa-4ae6-4c04-899a-cb73453b2442"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""186036a9-d9aa-478c-a50c-d0c4d4a88257"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""Button"",
                    ""id"": ""04e6b5fa-4e24-4744-be2f-974a17e9d13b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7bd959f9-934a-41f2-b456-dca0721108c9"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f7f2856e-bc27-4046-8af4-34d86b02790b"",
                    ""path"": ""<Gamepad>/select"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Default"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""584e7b76-10ea-4c72-85d1-3f8a7b791b4a"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Default"",
            ""bindingGroup"": ""Default"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
            // Navigation
            m_Navigation = asset.FindActionMap("Navigation", throwIfNotFound: true);
            m_Navigation_Select = m_Navigation.FindAction("Select", throwIfNotFound: true);
            m_Navigation_Cancel = m_Navigation.FindAction("Cancel", throwIfNotFound: true);
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

        // Navigation
        private readonly InputActionMap m_Navigation;
        private INavigationActions m_NavigationActionsCallbackInterface;
        private readonly InputAction m_Navigation_Select;
        private readonly InputAction m_Navigation_Cancel;
        public struct NavigationActions
        {
            private @NavigationInputCollection m_Wrapper;
            public NavigationActions(@NavigationInputCollection wrapper) { m_Wrapper = wrapper; }
            public InputAction @Select => m_Wrapper.m_Navigation_Select;
            public InputAction @Cancel => m_Wrapper.m_Navigation_Cancel;
            public InputActionMap Get() { return m_Wrapper.m_Navigation; }
            public void Enable() { Get().Enable(); }
            public void Disable() { Get().Disable(); }
            public bool enabled => Get().enabled;
            public static implicit operator InputActionMap(NavigationActions set) { return set.Get(); }
            public void SetCallbacks(INavigationActions instance)
            {
                if (m_Wrapper.m_NavigationActionsCallbackInterface != null)
                {
                    @Select.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
                    @Select.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
                    @Select.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnSelect;
                    @Cancel.started -= m_Wrapper.m_NavigationActionsCallbackInterface.OnCancel;
                    @Cancel.performed -= m_Wrapper.m_NavigationActionsCallbackInterface.OnCancel;
                    @Cancel.canceled -= m_Wrapper.m_NavigationActionsCallbackInterface.OnCancel;
                }
                m_Wrapper.m_NavigationActionsCallbackInterface = instance;
                if (instance != null)
                {
                    @Select.started += instance.OnSelect;
                    @Select.performed += instance.OnSelect;
                    @Select.canceled += instance.OnSelect;
                    @Cancel.started += instance.OnCancel;
                    @Cancel.performed += instance.OnCancel;
                    @Cancel.canceled += instance.OnCancel;
                }
            }
        }
        public NavigationActions @Navigation => new NavigationActions(this);
        private int m_DefaultSchemeIndex = -1;
        public InputControlScheme DefaultScheme
        {
            get
            {
                if (m_DefaultSchemeIndex == -1) m_DefaultSchemeIndex = asset.FindControlSchemeIndex("Default");
                return asset.controlSchemes[m_DefaultSchemeIndex];
            }
        }
        public interface INavigationActions
        {
            void OnSelect(InputAction.CallbackContext context);
            void OnCancel(InputAction.CallbackContext context);
        }
    }
}
