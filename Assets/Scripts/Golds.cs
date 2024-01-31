using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golds : MonoBehaviour
{
    public int goldWorth = 1;
    private Rigidbody rb;
    private Collider collider;
    private Transform player;
    private float speed = 0.05f;
    private float followTimer = 2f;
    private bool followingPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        collider = transform.parent.GetComponent<Collider>();
        player = FindObjectOfType<PlayerMotor>().transform;
        Invoke("FollowPlayer", followTimer);
    }

    // Update is called once per frame
    void Update()
    {
        if(followingPlayer){
            transform.parent.position = Vector3.MoveTowards(transform.parent.position, player.position, speed);
        }
    }

    public void FollowPlayer(){
        followingPlayer = true;
        collider.isTrigger = true;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")){
            other.GetComponent<PlayerStatus>().AddGold(goldWorth);
            // Destroy(transform.parent.gameObject);
            transform.parent.gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
        }
    }
}
