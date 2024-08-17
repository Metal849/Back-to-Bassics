using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public GameObject shadow;
    public RaycastHit hit;
    public float offset;
    private void FixedUpdate()
    {
        Ray downRay = new Ray(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), -Vector3.up);
        Vector3 hitposition = hit.point;
        shadow.transform.position = hitposition;
        Physics.Raycast(downRay, out hit);
    }
}
