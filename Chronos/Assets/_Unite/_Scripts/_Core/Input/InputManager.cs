using Unite.EventSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Unite.Core.Input
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }
        
        [Header("Event for interact action")]
        [SerializeField] 
        private GameEvent onPlayerInteractAction;

        [Header("Event for use ability action")]
        [SerializeField] 
        private GameEvent onPlayerUseAbilityAction;

        [Header("Events for journal actions")]
        [SerializeField]
        private GameEvent onJournalOpenAction;

        [SerializeField]
        private GameEvent onJournalCloseAction;
        
        private PlayerInputActions playerInput;
        private PlayerInputActions.DefaultActions defaultActions;
        private PlayerInputActions.UIActions uiActions;

        public PlayerInputActions PlayerInput => playerInput;
        public PlayerInputActions.DefaultActions DefaultActions => defaultActions;
        public PlayerInputActions.UIActions UIActions => uiActions;
        
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("More than one instance of InputManager in the scene! Destroying current instance.");
                Destroy(this);
            }

            Instance = this;
            
            playerInput = new PlayerInputActions();
            defaultActions = playerInput.Default;
            uiActions = playerInput.UI;
            
            SwitchToDefaultActionMap();
        }

        private void OnEnable()
        {
            SubscribeToActions();
        }

        private void OnDisable()
        {
            UnsubscribeToActions();
        }
        
        private void RaisePlayerUseAbilityEvent(InputAction.CallbackContext ctx)
        {
            onPlayerUseAbilityAction.Raise();
        }

        private void RaisePlayerInteractEvent(InputAction.CallbackContext ctx)
        {
            onPlayerInteractAction.Raise();
        }

        private void RaiseJournalOpenEvent(InputAction.CallbackContext ctx)
        {
            onJournalOpenAction.Raise();
        }
        
        private void RaiseJournalCloseEvent(InputAction.CallbackContext ctx)
        {
            onJournalCloseAction.Raise();
        }

        private void SubscribeToActions()
        {
            defaultActions.Ability1.performed += RaisePlayerUseAbilityEvent;
            defaultActions.Interact.performed += RaisePlayerInteractEvent;
            defaultActions.JournalOpen.performed += RaiseJournalOpenEvent;

            uiActions.CloseJournal.performed += RaiseJournalCloseEvent;
        }

        private void UnsubscribeToActions()
        {
            defaultActions.Ability1.performed -= RaisePlayerUseAbilityEvent;
            defaultActions.Interact.performed -= RaisePlayerInteractEvent;
            defaultActions.JournalOpen.performed -= RaiseJournalOpenEvent;

            uiActions.CloseJournal.performed -= RaiseJournalCloseEvent;
        }
        
        public bool IsShootActionPressed() => defaultActions.Shoot.IsPressed();

        public void SwitchToDefaultActionMap()
        {
            defaultActions.Enable();
            uiActions.Disable();
        }
        
        public void SwitchToUIActionMap()
        {
            defaultActions.Disable();
            uiActions.Enable();
        }
    }
}