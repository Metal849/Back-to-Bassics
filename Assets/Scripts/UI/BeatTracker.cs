using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatTracker : Conductable
{
    private TextMeshProUGUI _textMesh;
    protected override void Start()
    {
        base.Start();
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    protected override void OnFullBeat()
    {
        _textMesh.text = "Beat: " + Conductor.Instance.Beat;
    }
}
