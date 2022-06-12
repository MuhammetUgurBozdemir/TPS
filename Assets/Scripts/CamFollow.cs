using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LerpType lerpType;
    [SerializeField] private float smoothDampTime;
    [SerializeField] private Vector3 offset;
    private Vector3 velocity = Vector3.zero;


    [SerializeField] private Vector3 endCamOffset;
    public enum LerpType
    {
        SmoothDamp,
        Lerp,
        Slerp,
        Null,
    }
    void Start()
    {
        SetOffset();
    }


    private void FixedUpdate()
    {
        Vector3 pos = target.position - offset;
        if (lerpType == LerpType.Lerp)
        {
            transform.position = Vector3.Lerp(transform.position, pos, 5f * Time.deltaTime);
        }
        else if (lerpType == LerpType.SmoothDamp)
        {
            transform.position = Vector3.SmoothDamp(transform.position, pos, ref velocity, smoothDampTime);
        }
        else if (lerpType == LerpType.Slerp)
        {
            transform.position = Vector3.Slerp(transform.position, pos, 5);
        }
        else
        {
            transform.position = pos;
        }

    }

    private void SetOffset()
    {
        offset = target.position - transform.position;
        endCamOffset = offset;
    }
}
