using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMotor : MonoBehaviour
{
    GameObject player;
    EnemyStatus enemyStatus;

    private NavMeshAgent navMesh;
    public Transform target;
    public Animator enemyAnimator;
    public AnimatorManager aManager;
    public float walkSpeed = 1.5f;
    public float speed = 3.5f;
    public bool isStopping = false;
    public bool detectTarget = false;
    public bool hasKilled = false;
    private Rigidbody[] ragdollRigidbodies;
    private Collider[] ragdollColliders;
    private Rigidbody rigidbody;

    //Random movement
    public float range; //radius of sphere
    public Transform centrePoint; //centre of the area the agent wants to move around in
    //instead of centrePoint you can set it as the transform of the agent if you don't care about a specific area
    
    [SerializeField]
    private Vector3 enemyVelocity;
    [SerializeField]
    private float enemySpeed;

    public EnemyItemDropper dropper;

    void Awake(){
        navMesh = GetComponent<NavMeshAgent>();
        rigidbody = GetComponent<Rigidbody>();
        ragdollRigidbodies = transform.Find("Zombie1").GetComponentsInChildren<Rigidbody>();
        ragdollColliders = transform.Find("Zombie1").GetComponentsInChildren<Collider>();
        DisableRagdoll();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        enemyStatus = GetComponent<EnemyStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        enemyVelocity = navMesh.velocity;
        enemySpeed = Mathf.Sqrt(Mathf.Pow(enemyVelocity.x,2) + Mathf.Pow(enemyVelocity.z,2));
        
        if(hasKilled){
            
            navMesh.enabled = false;
            rigidbody.constraints = RigidbodyConstraints.None;
        }else{
            if(isStopping){
                navMesh.speed = 0;
            }else{
                if(detectTarget){
                    if(enemySpeed > 3f){
                        // aManager.ChangeAnimationState("Z_Run_InPlace");
                    }else{
                        // aManager.ChangeAnimationState("Z_Walk_InPlace");
                    }
                    navMesh.speed = speed;
                    navMesh.destination = target.position;
                }else{
                    navMesh.speed = walkSpeed;
                    if(navMesh.remainingDistance <= navMesh.stoppingDistance) //done with path
                    {
                        Vector3 point;
                        if (RandomPoint(centrePoint.position, range, out point)) //pass in our centre point and radius of area
                        {
                            Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f); //so you can see with gizmos
                            navMesh.SetDestination(point);
                        }
                    }else{
                        // aManager.ChangeAnimationState("Z_Walk_InPlace");
                    }
                }
            }

            aManager.SetMagnitude("InputMagnitude", enemySpeed/speed, 0.05f);
        }

        // if(aManager.GetAnimationState() == "Z_Walk_InPlace"){
        //     aManager.SetAnimationSpeed(enemySpeed/walkSpeed);
        // }else{
        //     aManager.SetAnimationSpeed(1);
        // }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "AttackPlayer"){
            if(player.GetComponent<PlayerAttack>().isAttacking && !enemyStatus.isInvincible){
                enemyAnimator.SetTrigger("Damage");
                enemyStatus.Damage(player.GetComponent<PlayerAttack>().attackDamage);
            }
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        // Debug.Log(randomPoint);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas)) //documentation: https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            //or add a for loop like in the documentation
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    public void Knockback(float force){
        dropper.SpawnRequiredGolds();
        Vector3 direction = transform.position - player.transform.position;
        EnableRagdoll();
        direction.Normalize();
        Debug.Log(direction);
        rigidbody.AddForce(new Vector3(direction.x * force, 750f, direction.z * force));
    }

    public void DisableRagdoll(){
        foreach(var rb in ragdollRigidbodies){
            rb.isKinematic = true;
        }
        foreach(var collider in ragdollColliders){
            collider.isTrigger = true;
        }
    }

    public void EnableRagdoll(){
        foreach(var rb in ragdollRigidbodies){
            rb.isKinematic = false;
        }
        foreach(var collider in ragdollColliders){
            collider.isTrigger = false;
        }
    }

}
