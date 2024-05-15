using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerBattlePawn : BattlePawn
{
    [Header("Player References")]
    [SerializeField] private DrawSpace _drawSpace;
    private bool blocking;
    public Direction CurrSlashDirection { get; private set; }
    private int lastSlashBeat = 0;
    protected override void Awake()
    {
        base.Awake();
        CurrSlashDirection = Direction.None;
    }
    public void Block()
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle") || blocking) return;
        blocking = true;
        _spriteAnimator.Play("Block");
    }
    public void Unblock()
    {  
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Block") || !blocking) return;
        blocking = false;
        _spriteAnimator.Play("Unblock");
    }
    public void Dodge(Direction direction)
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        switch (direction)
        {
            case Direction.West:
                _spriteAnimator.Play("DodgeWest");
                break;
            case Direction.East:
                _spriteAnimator.Play("DodgeEast");
                break;
            case Direction.South:
                _spriteAnimator.Play("DodgeSouth");
                break;
            default:
                Debug.LogError("Not Valid Dodge Direction Inputed");
                break;
        }
    }
    /// <summary>
    /// Slash with an amount of strength in a specified cardinal direction
    /// </summary>
    /// <param name="strength"></param>
    /// <param name="direction"></param>
    public void Slash(float strength, Direction direction)
    {
        AnimatorStateInfo animatorState = _spriteAnimator.GetCurrentAnimatorStateInfo(0);
        if (!animatorState.IsName("Idle")) return;
        CurrSlashDirection = direction;
        lastSlashBeat = Conductor.Instance.Beat;
        Debug.Log("Slash at " + Conductor.Instance.Beat);
        switch (direction)
        {
            case Direction.North:
                Debug.Log("North");
                break;
            case Direction.South:
                Debug.Log("South");
                break;
            case Direction.East:
                Debug.Log("East");
                break;
            case Direction.West:
                Debug.Log("West");
                break;
            case Direction.Northeast:
                Debug.Log("Northeast");
                break;
            case Direction.Northwest:
                Debug.Log("Northwest");
                break;
            case Direction.Southeast:
                Debug.Log("Southeast");
                break;
            case Direction.Southwest:
                Debug.Log("Southwest");
                break;
        }
    }
    private void Update()
    {
        // Legacy Input System Memes
        if (Input.GetKeyDown(KeyCode.A))
        {
            Dodge(Direction.West);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Dodge(Direction.East);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Dodge(Direction.South);
        }
        if (Input.GetMouseButton(1))
        {
            Block();
        }
        if (Input.GetMouseButtonUp(1))
        {
            Unblock();
        }
    }
    protected override void OnBeat()
    {
        if (CurrSlashDirection != Direction.None && Conductor.Instance.Beat > lastSlashBeat)
        {
            CurrSlashDirection = Direction.None;
            Debug.Log("Slash complete at " + Conductor.Instance.Beat);
        }
    }
}
