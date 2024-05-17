using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Data/PlayerWeaponData"), System.Serializable]
public class PlayerWeaponData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _dmg;
    [SerializeField] private int _lrch;
    [SerializeField, TextArea] private string _lore;

    public string Name => _name;
    public int Dmg => _dmg;
    public int Lrch => _lrch;
    public string Lore => _lore;
}
