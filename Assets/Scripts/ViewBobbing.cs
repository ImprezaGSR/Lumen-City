using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    public float effectIntensity;
    public float effectIntensityX;
    public float effectSpeed;

    private PositionFollower followerInstance;
    private Vector3 originalOffset;
    private float sinTime;

    // Start is called before the first frame update
    void Start()
    {
        followerInstance = GetComponent<PositionFollower>();
        originalOffset = followerInstance.offset;
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void ProcessBobbing(Vector2 input){
        Vector3 inputVector = new Vector3(input.x, 0, input.y);
        if(inputVector.magnitude > 0f){
            sinTime += Time.deltaTime + effectSpeed;
        }else{
            sinTime = 0f;
        }
        float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(sinTime));
        Vector3 sinAmountX = followerInstance.transform.right * effectIntensity * Mathf.Cos(sinTime) * effectIntensityX;
        followerInstance.offset = new Vector3{
            x = originalOffset.x,
            y = originalOffset.y + sinAmountY,
            z = originalOffset.z
        };
        followerInstance.offset += sinAmountX;
    }
}
