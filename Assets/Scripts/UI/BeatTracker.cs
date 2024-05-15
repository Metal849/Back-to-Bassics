using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BeatTracker : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    private void Start()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }
    private void FixedUpdate()
    {
        _textMesh.text = "Beat: " + Conductor.Instance.Beat;
    }
}
