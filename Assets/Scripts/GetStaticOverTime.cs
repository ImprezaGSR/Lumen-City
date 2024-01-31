using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetStaticOverTime : MonoBehaviour
{
    public float duration = 4f;
    public bool isSolid = false;
    private Rigidbody rb;
    private Collider collider;
    private bool isColliding = false;

    void Start(){
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        StartCoroutine(GetStatic());
    }

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == ("Island")){
            isColliding = true;
            Debug.Log("isColliding");
        }
    }
    void OnCollisionExit(Collision other){
        if(other.gameObject.tag == ("Island")){
            isColliding = false;
            Debug.Log("isNotColliding");
        }
    }
    IEnumerator GetStatic(){
        yield return new WaitForSeconds(duration);
        if(isColliding){
            rb.isKinematic = true;
            if(!isSolid){
                collider.isTrigger = true;
            }
        }else{
            Destroy(gameObject);
        }
    }
}
