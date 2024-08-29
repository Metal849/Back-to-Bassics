using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfigure : Singleton<CameraConfigure>
{
    [SerializeField] private CinemachineVirtualCamera firstVirtualCamera;
    private CinemachineVirtualCamera curr;
    private int savedPriority;
    private CinemachineVirtualCamera prev;
    #region Unity Messages
    private void Awake()
    {
        InitializeSingleton();
        curr = firstVirtualCamera;
        prev = curr;
        curr.Priority = 10;
    }
    #endregion
    public void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        //curr.Priority = savedPriority;
        curr.Priority = 1;
        prev = curr;
        curr = targetCamera;
        //savedPriority = curr.Priority;
        curr.Priority = 10;
        Debug.Log($"Curr: {curr}, Prev: {prev}");
    }
    public void SwitchBackToPrev()
    {
        SwitchToCamera(prev);
    }
}
