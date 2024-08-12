using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Rewrite this trash, its ugly to read
/// </summary>
public partial class UIManager
{
    [Header("Battle Panel")]
    [SerializeField] private TextMeshProUGUI parryTracker;
    [SerializeField] private TextMeshProUGUI blockTracker;
    [SerializeField] private TextMeshProUGUI missTracker;
    [SerializeField] private TextMeshProUGUI beatTracker;
    [SerializeField] private TextMeshProUGUI centerText;
    [SerializeField] private Animator battlePanelAnimator;
    [SerializeField] private Image _playerHpBar;
    [SerializeField] private Image _enemyHpBar;
    [SerializeField] private Image _comboMeterBar;
    int parryCount;
    int blockCount;
    int missCount;
    // Replacement For easier use maybe
    //public GaugeTracker PlayerHP;
    //public GaugeTracker EnemyHP;
    //public GaugeTracker ComboMeter;
    private void FixedUpdate()
    {
        beatTracker.text = $"Beat: {(int)Conductor.Instance.Beat}";
    }
    public void IncrementParryTracker()
    {
        parryTracker.text = $"Parries: {++parryCount}";
    }
    public void IncrementBlockTracker()
    {
        blockTracker.text = $"Blocks: {++blockCount}";
    }
    public void IncrementMissTracker()
    {
        missTracker.text = $"Misses: {++missCount}";
    }
    public void UpdateCenterText(string text)
    {
        centerText.text = text;
    }
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
    public void UpdateComboMeter()
    {
        // Combo Meter increase
        _comboMeterBar.fillAmount = 1f;
    }
    //public void UpdateSP(BattlePawn pawn)
    //{  
    //    if (pawn is PlayerBattlePawn)
    //    {
    //        // Player Pawn
    //        _playerSpBar.fillAmount = (float)pawn.SP / pawn.Data.SP;
    //        return;
    //    }
    //    // Enemy Pawn
    //    _enemySpBar.fillAmount = (float)pawn.SP / pawn.Data.SP;
    //}
    public void ShowBattlePanel()
    {
        battlePanelAnimator.Play("ShowBattlePanel");
    }
    public void HideBattlePanel()
    {
        battlePanelAnimator.Play("HideBattlePanel");
    }
}
