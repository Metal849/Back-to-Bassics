using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatTracker : Conductable
{
    [SerializeField] private TextMeshProUGUI _textMesh;
    protected override void OnFullBeat()
    {
        _textMesh.text = "Beat: " + Conductor.Instance.Beat;
    }
}
