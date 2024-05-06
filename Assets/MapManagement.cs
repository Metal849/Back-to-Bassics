using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MapManagement : MonoBehaviour
{
    [SerializeField] private GameObject stage;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject button;
    [SerializeField] private MapNode[] nodes;

    public void StartButton() {
        stage.SetActive(true);
        enemy.SetActive(true);
        button.SetActive(false);
    }

    public void NextButton() {
        
    }

}
