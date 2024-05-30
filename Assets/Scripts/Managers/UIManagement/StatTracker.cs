using UnityEngine;
using UnityEngine.UI;

public partial class UIManager
{
    [Header("Stat Tracker")]
    [SerializeField] private Image _playerHpBar;
    [SerializeField] private Image _playerSpBar;

    [SerializeField] private Image _enemyHpBar;
    [SerializeField] private Image _enemySpBar;
    public void UpdateHP(BattlePawn pawn)
    {     
        if (pawn is PlayerBattlePawn)
        {
            // Player Pawn
            _playerHpBar.fillAmount = (float)pawn.HP / pawn.Data.HP;
            return;
        }

        // Enemy Pawn
        _enemyHpBar.fillAmount = (float)pawn.HP / pawn.Data.HP;
    }
    public void UpdateSP(BattlePawn pawn)
    {  
        if (pawn is PlayerBattlePawn)
        {
            // Player Pawn
            _playerSpBar.fillAmount = (float)pawn.SP / pawn.Data.SP;
            return;
        }
        // Enemy Pawn
        _enemySpBar.fillAmount = (float)pawn.SP / pawn.Data.SP;
    }
}
