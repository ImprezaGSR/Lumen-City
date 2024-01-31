using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{

    public Transform attackTrans;
    public float attackStartRange = 1.25f;

    public Animator animator;
    public Animator zombieAnimator;
    public AnimatorManager aManager;

    EnemyMotor motor;
    [SerializeField]
    private float targetDistance;

    private bool canAttack = true;
    public bool isAttacking = false;
    public float attackDuration = 0.3f;
    public float attackCooldown = 1f;

    //Melee Attack Loop
    public int attackLoops = 2;
    private int remainingLoops;
    public float loopCooldown;

    

    // Start is called before the first frame update
    void Start()
    {
        motor = GetComponent<EnemyMotor>();
        remainingLoops = attackLoops;
    }

    // Update is called once per frame
    void Update()
    {
        targetDistance = Vector3.Distance(motor.target.position, attackTrans.position);
        
        if(!motor.hasKilled && targetDistance <= attackStartRange){
            if(canAttack){
                MeleeAttack();
            }
            
        }

        // if(isAttacking){
        //     zombieAnimator.SetFloat("AttackMagnitude", 1, 0.5f, Time.deltaTime);
        // }else{
        //     zombieAnimator.SetFloat("AttackMagnitude", 0, 0.5f, Time.deltaTime);
        // }
    }

    void MeleeAttack(){
        if(!isAttacking){
            isAttacking = true;
            motor.isStopping = true;
            animator.SetTrigger("Attack");
            if(canAttack){
                aManager.SetAnimationBool("isAttacking", true);
                aManager.SetAnimationLayerWeight(1, 1f, 0.05f);
                canAttack = false;
            }
            // aManager.ChangeAnimationState("Z_Attack");
            if(remainingLoops <= 0){
                StartCoroutine(ResetAttackCooldown());
            }else{
                StartCoroutine(LoopAttackCooldown());
            }
        }
    }

    IEnumerator ResetAttackCooldown(){
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        aManager.SetAnimationBool("isAttacking", false);
        // aManager.SetMagnitude("AttackMagnitude", 0, 0.05f);
        aManager.SetAnimationLayerWeight(1, 0, 0.5f);
        yield return new WaitForSeconds(attackCooldown);
        motor.isStopping = false;
        motor.detectTarget = true;
        remainingLoops = attackLoops;
        canAttack = true;
    }

    IEnumerator LoopAttackCooldown(){
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
        remainingLoops -= 1;
        MeleeAttack();
    }

    
}
