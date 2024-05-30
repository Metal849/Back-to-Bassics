using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public partial class UIManager : Singleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI parryTracker;
    [SerializeField] private TextMeshProUGUI blockTracker;
    [SerializeField] private TextMeshProUGUI missTracker;
    int parryCount;
    int blockCount;
    int missCount;
    private void Awake()
    {
        InitializeSingleton();
    }
    public void IncrementParryTracker()
    {
        parryTracker.text = "Parries: " + (++parryCount);
    }
    public void IncrementBlockTracker()
    {
        blockTracker.text = "Blocks: " + (++blockCount);
    }
    public void IncrementMissTracker()
    {
        missTracker.text = "Misses: " + (++missCount);
    }
}
