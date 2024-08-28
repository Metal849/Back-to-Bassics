using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfigure : Singleton<CameraConfigure>
{
    private CinemachineVirtualCamera[] prev;
    private CinemachineVirtualCamera curr;
    private int[] prevPriority;
    private int currOgPriority;
    #region Unity Messages
    private void Awake()
    {
        InitializeSingleton();
        curr = CinemachineBrain.SoloCamera as CinemachineVirtualCamera;
        prevPriority = new int[2];
        prev = new CinemachineVirtualCamera[] { curr, curr };
    }
    #endregion
    public void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        prevPriority[1] = prev[0].Priority;
        prev[1] = prev[0];
        prev[1].Priority = 0;

        prevPriority[0] = curr.Priority;
        prev[0] = curr;
        prev[0].Priority = 0;

        curr = targetCamera;
        currOgPriority = curr.Priority;
        curr.Priority = 100;   
    }
    public void SwitchBackToPrev()
    {
        prev[1].Priority = prevPriority[1];
        prev[0].Priority = prevPriority[0];
        prev[]
    }
}
