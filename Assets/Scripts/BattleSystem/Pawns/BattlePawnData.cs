using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BattlePawnData"), System.Serializable]
public class BattlePawnData : ScriptableObject
{
    [SerializeField, Tooltip("Name of pawn")] private string _name;
    [SerializeField, Tooltip("Hit points")] private int _hp;
    [SerializeField, Tooltip("Stagger points")] private int _sp;
    [SerializeField, Tooltip("Multiple per fixedDeltaTime")] private float _spRecoveryRate;
    [SerializeField, Tooltip("In seconds")] private float _staggerRecoveryTime;
    [SerializeField, TextArea] private string _lore;

    public string Name => _name;
    public int HP => _hp;
    public int SP => _sp;
    public float SPRecoveryRate => _spRecoveryRate;
    public float StaggerRecoveryTime => _staggerRecoveryTime;
    public string Lore => _lore;
}
