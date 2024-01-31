using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    private Vector3 localPos;
    // Start is called before the first frame update
    void Start()
    {
        localPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = localPos;
    }
}
