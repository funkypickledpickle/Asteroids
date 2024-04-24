using Asteroids.Installation;
using Asteroids.UI.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Zenject;

namespace Asteroids.UI.Views
{
    [RequireComponent(typeof(Injector))]
    public class KeyButton : Button, IInjectable
    {
        [SerializeField] private bool _isCancelButtonEnabled;

        [Inject] private readonly NavigationInputCollection NavigationInputCollection;

        protected override void OnEnable()
        {
            base.OnEnable();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            FixEventSystemState();

            NavigationInputCollection.Enable();

            NavigationInputCollection.Navigation.@Select.performed += SelectPerformedHandler;
            if (_isCancelButtonEnabled)
            {
                NavigationInputCollection.Navigation.@Cancel.performed += CancelPerformedHandler;
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                return;
            }
#endif

            NavigationInputCollection.Disable();

            NavigationInputCollection.Navigation.@Select.performed -= SelectPerformedHandler;
            if (_isCancelButtonEnabled)
            {
                NavigationInputCollection.Navigation.@Cancel.performed -= CancelPerformedHandler;
            }
        }

        /// <summary>
        /// ExecuteEvents.submitHandler is automatically discharged by InputSystemUIInputModule. This happens if the button was pressed by mouse and was disabled on the same frame.
        /// This is needed to clean current selected gameObject in Event
        /// </summary>
        /// <returns></returns>
        private void FixEventSystemState()
        {
            interactable = false;
            interactable = true;
        }

        private void SelectPerformedHandler(InputAction.CallbackContext context)
        {
            HandlePressed();
        }

        private void CancelPerformedHandler(InputAction.CallbackContext context)
        {
           HandlePressed();
        }

        private void HandlePressed()
        {
            if (!IsActive() || !IsInteractable())
            {
                return;
            }

            onClick.Invoke();
        }
    }
}
