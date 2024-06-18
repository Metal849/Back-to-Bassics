using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerinput;
    private PlayerBattlePawn _battlepawn;
    private PlayerTraversalPawn _traversalpawn;
    private void Awake()
    {
        // References
        _playerinput = GetComponent<PlayerInput>();
        _battlepawn = GetComponent<PlayerBattlePawn>();
        _traversalpawn = GetComponent<PlayerTraversalPawn>();

        // Input Battle Actions
        _playerinput.SwitchCurrentActionMap("PlayerBattlePawn");
        _playerinput.actions["Dodge"].performed += OnDodge;
        _playerinput.actions["Jump"].performed += OnDodge;
        //_playerinput.actions["Block"].performed += OnBlock;
        //_playerinput.actions["Block"].canceled += OnBlock;
        _playerinput.actions["Slash"].performed += OnBattleSlash;

        // Input World Traversal Actions
        // This might just have to keep updating on fixed update tbh
        _playerinput.SwitchCurrentActionMap("PlayerTraversalPawn");
        //_playerinput.actions["Move"].performed += OnMove;
        _playerinput.actions["Slash"].performed += OnTraversalSlash;
    }
    public void SwitchToBattleActions()
    {
        _playerinput.currentActionMap.Disable();
        _playerinput.SwitchCurrentActionMap("PlayerBattlePawn");
        _playerinput.currentActionMap.Enable();
    }
    public void SwitchToTraversalActions()
    {
        _playerinput.currentActionMap.Disable();
        _playerinput.SwitchCurrentActionMap("PlayerTraversalPawn");
        _playerinput.currentActionMap.Enable();
    }
    #region Battle Pawn Actions
    public void OnDodge(InputAction.CallbackContext context)
    {
        _battlepawn.Dodge(context.ReadValue<Vector2>());
    }
    //public void OnBlock(InputAction.CallbackContext context)
    //{
    //    if (context.performed)
    //    {
    //        _battlepawn.Block();
    //    }
    //    if (context.canceled)
    //    {
    //        _battlepawn.Unblock();
    //    }
    //}
    public void OnBattleSlash(InputAction.CallbackContext context)
    {
        _battlepawn.Slash(context.ReadValue<Vector2>());
    }
    #endregion
    #region Traversal Pawn Actions
    private void Update()
    {
        Vector2 direction = _playerinput.actions["Move"].ReadValue<Vector2>();
        _traversalpawn.Move(new Vector3(direction.x, 0, direction.y));
    }
    //public void OnMove(InputAction.CallbackContext context)
    //{
        
    //}
    public void OnJump(InputAction.CallbackContext context)
    {

    }
    public void OnTraversalSlash(InputAction.CallbackContext context)
    {
        _traversalpawn.Slash(context.ReadValue<Vector2>());
    }
    #endregion
}
