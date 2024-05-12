using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/BattlePawnData"), System.Serializable]
public class BattlePawnData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _hp;
    [SerializeField] private int _sp;
    [SerializeField, TextArea] private string _lore;

    public string Name => _name;
    public int HP => _hp;
    public int SP => _sp;
    public string Lore => _lore;
}
