using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reticle : MonoBehaviour
{
    [SerializeField]
    private Camera _cameraFacing;

    private Vector3 _localScale;

    private void Start()
    {
        _localScale = transform.localScale;
    }

    void Update()
    {
       
        RaycastHit raycastHit;
        float distance;
        if(Physics.Raycast(new Ray(_cameraFacing.transform.position, _cameraFacing.transform.rotation * Vector3.forward), out raycastHit))
        {
            distance = raycastHit.distance;
        }
        else
        {
            distance = _cameraFacing.farClipPlane * 0.95f;
        }

        transform.LookAt(_cameraFacing.transform.position);
        transform.Rotate(0, 180, 0);
        transform.position = _cameraFacing.transform.position + _cameraFacing.transform.rotation * Vector3.forward * distance;
        transform.localScale = _localScale * distance;
    }
}
