using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/EnemyBattlePawnData"), System.Serializable]
public class EnemyBattlePawnData : BattlePawnData
{
    [SerializeField] private int _bpm;
    
    public int BPM => _bpm; 
}
