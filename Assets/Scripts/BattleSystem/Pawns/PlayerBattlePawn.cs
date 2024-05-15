using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using TMPro;

public class PlayerBattlePawn : BattlePawn
{
    [Header("Player References")]
    [SerializeField] private DrawSpace _drawSpace;
    [SerializeField] private TextMeshProUGUI _slashText;
    public bool blocking { get; private set; }
    public Direction CurrSlashDirection { get; private set; }
    private float lastSlashBeat = 0;
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
        _slashText.text = CurrSlashDirection.ToString() + " slash at beat " + Conductor.Instance.Beat;
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
    public void ReceiveAttackRequest()
    {
        CurrSlashDirection = Direction.None;
    }
    //protected override void OnQuarterBeat()
    //{
    //    if (CurrSlashDirection != Direction.None && Conductor.Instance.Beat >= lastSlashBeat)
    //    {
    //        CurrSlashDirection = Direction.None;
    //        _slashCompleteText.text = "Slash complete at beat " + Conductor.Instance.Beat;
    //    }
    //}
}
