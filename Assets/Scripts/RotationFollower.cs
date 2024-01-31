using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationFollower : MonoBehaviour
{
    public Transform targetTransform;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.rotation = targetTransform.rotation;
    }
}
