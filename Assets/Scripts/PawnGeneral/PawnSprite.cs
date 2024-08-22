using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PawnSprite : MonoBehaviour
{
    public Animator Animator => _animator;
    public Vector3 FacingDirection => _facingDirection;
    protected Animator _animator;
    protected Vector3 _facingDirection;
    protected SpriteRenderer _spriteRenderer;
    protected virtual void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _facingDirection = new Vector3(_animator.GetFloat("x_faceDir"), 0, _animator.GetFloat("z_faceDir"));
    }
    protected virtual void Start()
    {
        _spriteRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
    }
    public void FaceDirection(Vector3 direction)
    {
        if (direction.x != 0)
        {
            _animator.SetFloat("x_faceDir", Mathf.Sign(direction.x));
        }
        if (direction.z != 0)
        {
            _animator.SetFloat("z_faceDir", Mathf.Sign(direction.z));
        }
        Vector2 change = new Vector2(_animator.GetFloat("x_faceDir"), _animator.GetFloat("z_faceDir"));
        float angle = Vector2.SignedAngle(_facingDirection, change);
        if (angle > 0)
        {
            _animator.SetTrigger("flip_ccw");
        }
        else if (angle < 0)
        {
            _animator.SetTrigger("flip_cw");
        }
        _facingDirection = change;
    }
}
