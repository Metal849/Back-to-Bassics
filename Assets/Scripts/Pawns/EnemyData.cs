using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private int _hp;
    [SerializeField] private int _sp;
    [SerializeField] private int _bpm;
    [SerializeField] private GameObject[] _attacks;
    [SerializeField, TextArea] private string _lore;
    
    public string Name => _name;
    public int HP => _hp;
    public int SP => _sp;   
    public int BPM => _bpm; 
    public GameObject[] Attacks => _attacks;
    public string Lore => _lore; 
}
