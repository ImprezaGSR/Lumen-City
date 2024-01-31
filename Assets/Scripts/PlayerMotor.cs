using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{

    private static CharacterController controller;
    private Collider collider;
    public Vector3 playerVelocity;
    public Vector3 dodgeDirection;
    public float speed = 5f;
    public float slowSpeed = 1f;
    private bool isGrounded;
    public float gravity = -9.8f;
    public float jumpHeight = 3f;
    public float dodgeForce = 30f;
    public float runSpeed = 10f;
    public float dodgeDuration = 0.25f;
    public bool canDash = true;
    public bool isDodging = false; 
    [SerializeField]
    private bool isDashing = false;
    public bool isSlowing = false;
    public bool isStopping = false;
    private PlayerStatus status;
    private PlayerAttack attack;
    public bool canWarp = true;
    // Start is called before the first frame update
    void Start()
    {
        status = GetComponent<PlayerStatus>();
        controller = GetComponent<CharacterController>();
        collider = GetComponent<Collider>();
        attack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if(isDodging){
            controller.Move(transform.TransformDirection(dodgeDirection)*dodgeForce*Time.deltaTime);
            Debug.Log(dodgeDirection);
            Invoke("StopDodge",dodgeDuration);
            playerVelocity.y = 0;
            controller.detectCollisions = false;
        }else{
            controller.detectCollisions = true;
        }
    }
    //Receive the inputs from InputManager.cs
    public void ProcessMove(Vector2 input){
        if(!isStopping){
            Vector3 moveDirection = Vector3.zero;
            moveDirection.x = input.x;
            moveDirection.z = input.y;
            if(!isDodging){
                if(isDashing){
                    controller.Move(transform.TransformDirection(moveDirection)*runSpeed*Time.deltaTime);
                }else if(isSlowing){
                    controller.Move(transform.TransformDirection(moveDirection)*slowSpeed*Time.deltaTime);
                }else{
                    controller.Move(transform.TransformDirection(moveDirection)*speed*Time.deltaTime);
                }
                playerVelocity.y += gravity * Time.deltaTime;
                if(isGrounded && playerVelocity.y < 0){
                    playerVelocity.y = -2f;
                }
                controller.Move(playerVelocity * Time.deltaTime);
            }
            // Debug.Log(playerVelocity.y);
        }
    }
    public void Jump(){
        if(isGrounded){
            //Sqrt = Returns square root
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
    }
    public void Dash(Vector2 input){
        if(canDash){
            isDashing = true;
            isDodging = true;
            isSlowing = false;
            dodgeDirection = Vector3.zero;
            dodgeDirection.x = input.x;
            dodgeDirection.z = input.y;
            if(dodgeDirection.x == 0 && dodgeDirection.z == 0){
                dodgeDirection = new Vector3(0, 0, 1);
            }
        }
    }
    public void StopDash(){
        isDashing = false;
    }
    private void StopDodge(){
        isDodging = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit other)
    {
        if(other.gameObject.tag == "AttackEnemy"){
            if(other.transform.parent.parent.GetComponent<EnemyAttack>().isAttacking){
                status.Damage(other.transform.parent.parent.GetComponent<EnemyStatus>().attackDamage);
                Debug.Log(other.transform.parent.parent.GetComponent<EnemyStatus>().attackDamage+" Damage to Player");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "AttackEnemy"){
            if(other.transform.parent.parent.GetComponent<EnemyAttack>().isAttacking){
                if(attack.GetIsGuarding()){
                    attack.isAttacking = true;
                    attack.AddAttackDamage(other.transform.parent.parent.GetComponent<EnemyStatus>().attackDamage);
                }else{
                    status.Damage(other.transform.parent.parent.GetComponent<EnemyStatus>().attackDamage);
                    Debug.Log(other.transform.parent.parent.GetComponent<EnemyStatus>().attackDamage+" Damage to Player");
                }
            }
        }
        
    }
    

    // private void OnCollisionStay(Collision other){
    //     if(other.gameObject.tag == "Collectable"){
    //         status.AddGold(GetComponent<Golds>().goldWorth);
    //         Destroy(other.gameObject);
    //     }
    // }

    public void TransformPosition(Vector3 pos){
        // StartCoroutine(DisableCControllerForMoment());
        // collider.enabled = !collider.enabled;
        GetComponent<CharacterController>().enabled = !GetComponent<CharacterController>().enabled;
        StartCoroutine(DisableWarpForMoment());
        transform.position = pos;
        // collider.enabled = !collider.enabled;
        GetComponent<CharacterController>().enabled = !GetComponent<CharacterController>().enabled;
    }

    private IEnumerator DisableWarpForMoment(){
        canWarp = false;
        yield return new WaitForSeconds(0.5f);
        canWarp = true;
    }
}
