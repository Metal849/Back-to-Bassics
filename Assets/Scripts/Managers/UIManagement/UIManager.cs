using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[DisallowMultipleComponent]
public partial class UIManager : Singleton<UIManager>
{
    public void Awake()
    {
        InitializeSingleton();
    }
}
