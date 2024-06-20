using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyBattlePawnData"), System.Serializable]
public class EnemyBattlePawnData : BattlePawnData
{
    [Header("Enemy Data")]
    [SerializeField] private int _bpm;
    [SerializeField] private Vector3 _relativeBattleDistance;
    public int BPM => _bpm;
    public float SPB => 60f / _bpm;
    public Vector3 RelativeBattleDistance => _relativeBattleDistance;
}
