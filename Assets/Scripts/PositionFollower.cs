using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFollower : MonoBehaviour
{
    public Transform targetTransform;
    public Vector3 offset;
    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = targetTransform.position + offset;
    }
}
