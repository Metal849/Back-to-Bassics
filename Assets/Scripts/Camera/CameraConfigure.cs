using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraConfigure : MonoBehaviour
{
    private CinemachineVirtualCamera prev;
    public void SwitchToCamera(CinemachineVirtualCamera targetCamera)
    {
        prev = CinemachineBrain.SoloCamera as CinemachineVirtualCamera;
        CinemachineBrain.SoloCamera = targetCamera;
    }
    public void SwitchBackToPrev()
    {
        if (prev == null) return;
        CinemachineBrain.SoloCamera = prev;
    }
}
