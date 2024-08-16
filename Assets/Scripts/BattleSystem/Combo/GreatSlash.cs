using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combo/GreatSlash"), System.Serializable]
public class GreatSlash : Combo
{
    [SerializeField] private int damage;
    public int Damage => damage;
    public override void DoCombo()
    {
        BattleManager.Instance.Enemy.Damage(damage);
    }
}
