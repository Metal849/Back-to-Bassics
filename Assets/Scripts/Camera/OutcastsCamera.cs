using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutcastsCamera : Singleton<OutcastsCamera>
{

    [Header("Modifiers")]
    [SerializeField, Range(1f, 30f)] private float m_moveSpeed = 1f;
    [SerializeField] private Transform target;
    [SerializeField] public bool m_followEnabled = false;
    [SerializeField] public bool m_smooth = false;
    [SerializeField] private Vector3 offset;
    [SerializeField] private bool lockVertical;
    [SerializeField] private bool lockHorizontal;

    #region Technical

    #endregion
    private void Awake()
    {
        InitializeSingleton();
    }

    private void Update()
    {
        if (m_followEnabled && transform.position != target.position)
        {
            Vector3 destination = new Vector3(lockHorizontal ? 0 : target.position.x, lockVertical ? 0 : target.position.y, target.position.z) + offset;
            transform.position = m_smooth ? Vector3.LerpUnclamped(transform.position, destination, Time.deltaTime * m_moveSpeed * 0.1f) :
                Vector3.MoveTowards(transform.position, destination, m_moveSpeed * Time.deltaTime);
        }
    }
    public void FollowEnable(bool follow)
    {
        m_followEnabled = follow;
    }
    public void FollowSmooth(bool smooth)
    {
        m_smooth = smooth;
    }
    public void ChangeMoveSpeed(float speed)
    {
        m_moveSpeed = speed;
    }
}

