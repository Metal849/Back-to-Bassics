using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    private Camera primaryCamera;
    private void Start()
    {
        primaryCamera = FindObjectOfType<Camera>().GetComponent<Camera>();
    }

    private void FixedUpdate()
    {
        //transform.LookAt(primaryCamera.transform);

        transform.rotation = primaryCamera.transform.rotation;

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

}
