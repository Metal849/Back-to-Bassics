using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BattlePawnData"), System.Serializable]
public class BattlePawnData : ScriptableObject
{
    [Header("Battle Pawn Data")]
    [SerializeField, Tooltip("Name of pawn")] private string _name;
    [SerializeField, Tooltip("Hit points")] private int _hp;
    [SerializeField, Tooltip("Stagger points")] private int _sp;
    [SerializeField, Tooltip("Recovery every half beat")] private float _spRecoveryRate;
    [SerializeField, Tooltip("In seconds")] private float _staggerRecoveryTime;
    [SerializeField, TextArea] private string _lore;

    [Header("Resistances")]
    [SerializeField] private float _confuseResistance;
    [SerializeField] private float _poisonResistance;

    // Data Properties
    public string Name => _name;
    public int HP => _hp;
    public int SP => _sp;
    public float SPRecoveryRate => _spRecoveryRate;
    public float StaggerRecoveryTime => _staggerRecoveryTime;
    public string Lore => _lore;

    // Resistances
    public float ConfuseResistance => _confuseResistance;
    public float PoisonResistance => _poisonResistance;
}
