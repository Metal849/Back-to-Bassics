using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyBattlePawnData"), System.Serializable]
public class EnemyBattlePawnData : BattlePawnData
{
    [Header("Enemy Data")]
    [SerializeField] private int _bpm;
    [SerializeField] private int _blockLrch;
    public int BPM => _bpm;
    public int BlockLrch => _blockLrch;
}
