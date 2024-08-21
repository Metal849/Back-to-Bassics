using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using static PositionStateMachine;

public class PlayerTraversalPawn : TraversalPawn
{
    [SerializeField] private VisualEffect _slashEffect;
    [field: SerializeField] public EventReference slashSoundReference { get; private set; }
    private PlayerBattlePawn _battlePawn;
    private bool attacking;
    protected override void Awake()
    {
        base.Awake();
        _battlePawn = GetComponent<PlayerBattlePawn>();
    }
    public override void Move(Vector3 direction)
    {
        if (attacking) return;
        base.Move(direction);
    }
    public void Slash(Vector2 slashDirection)
    {
        if (attacking) return;
        _pawnSprite.FaceDirection(new Vector3(slashDirection.x, 0, 0));
        _pawnAnimator.Play($"Slash{DirectionHelper.GetVectorDirection(slashDirection)}");
        _slashEffect.Play();
        AudioManager.Instance.PlayOnShotSound(slashSoundReference, transform.position);
        // Set the Slash Direction
        //SlashDirection = direction;
        //SlashDirection.Normalize();
        //UIManager.Instance.PlayerSlash(SlashDirection);
        StartCoroutine(Attacking());
    }
    private IEnumerator Attacking()
    {
        attacking = true;
        yield return new WaitForSeconds(_battlePawn.WeaponData.AttackDuration /* * Conductor.quarter * Conductor.Instance.spb*/);
        attacking = false;
    }
}
