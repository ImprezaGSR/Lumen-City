using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private CharacterController controller;
    public GameObject WeaponHolder;
    public GameObject Sword;
    public PlayerMotor motor;
    PlayerStatus status;

    public float baseAttackDamage = 20;

    public float attackDamage = 0;
    
    public bool canAttack = true;
    public bool isAttacking = false;
    private bool isGuarding = false;

    public float attackXSamurai = 1f;
    public float attackCooldown = 0.5f;
    public float attackSecondCooldown = 0.2f;

    public float attackDashXSamurai = 1.5f;
    public float attackDashCooldown = 0.5f;
    
    private float damageStore = 0;
    public float counterX = 2f;
    public float skillRightCooldown = 1f;
    public float counterCooldown = 1f;
    public float hitBoxCooldown = 0.25f;
    public float guardSpeed = 1f;
    float attackSecondCooldownTemp;
    Animator swordAnim;
    public GameObject meleeHitBox;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        swordAnim = Sword.GetComponent<Animator>();
        status = GetComponent<PlayerStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if(attackSecondCooldownTemp > 0){
            attackSecondCooldownTemp -= Time.deltaTime;
        }
        swordAnim.SetFloat("AttackSecondCooldown", attackSecondCooldownTemp);
        if(motor.isDodging){
            isGuarding = false;
        }
        swordAnim.SetBool("isGuarding",isGuarding);
    }

    public void Attack(){
        if(canAttack && !motor.isDodging){
            attackDamage = baseAttackDamage * attackXSamurai;
            meleeHitBox.SetActive(false);
            isAttacking = true;
            canAttack = false;
            swordAnim.SetTrigger("Attack");
            StartCoroutine(ResetAttackCooldown());
        }
    }

    IEnumerator ResetAttackCooldown(){
        StartCoroutine(ResetHitBox());
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isAttacking = false;
        attackSecondCooldownTemp = attackSecondCooldown;
        attackDamage = 0;
    }

    public void AttackDash(){
        if(status.skillDashMeter >= status.skillDashMeterMax){
            attackDamage = baseAttackDamage * attackDashXSamurai;
            status.isInvincible = true;
            isAttacking = true;
            canAttack = false;
            swordAnim.SetTrigger("AttackDash");
            StartCoroutine(ResetAttackDashCooldown());
            status.skillDashMeter = 0;
        }
    }

    IEnumerator ResetAttackDashCooldown(){
        StartCoroutine(ResetHitBox());
        yield return new WaitForSeconds(attackDashCooldown);
        canAttack = true;
        isAttacking = false;
        status.isInvincible = false;
        attackDamage = 0;
    }

    public void SkillRight(){
        if(status.skillRightMeter >= status.skillRightMeterMax){
            if(!isAttacking && canAttack && !motor.isDodging){
                attackDamage = 0;
                isGuarding = true;
                motor.isSlowing = true;
                canAttack = false;
                status.isInvincible = true;
                swordAnim.SetTrigger("SkillRight");
                StartCoroutine(ResetSkillRightCooldown());
                status.skillRightMeter = 0;
            }
        }
    }

    IEnumerator ResetSkillRightCooldown(){
        yield return new WaitForSeconds(skillRightCooldown);
        if(isAttacking){
            Counter();
        }else{
            canAttack = true;
            attackDamage = 0;
        }
        motor.isSlowing = false;
        isGuarding = false;
    }

    public void Counter(){
        attackDamage *= counterX;
        motor.isStopping = true;
        motor.canDash = false;
        swordAnim.SetBool("isCountering",true);
        StartCoroutine(ResetCounterCooldown());
    }

    IEnumerator ResetCounterCooldown(){
        yield return new WaitForSeconds(0.4f);
        StartCoroutine(ResetHitBox());
        yield return new WaitForSeconds(counterCooldown);
        swordAnim.SetBool("isCountering",false);
        motor.isStopping = false;
        canAttack = true;
        isAttacking = false;
        motor.canDash = true;
        status.isInvincible = false;
        attackDamage = 0;
    }

    public void EnableMeleeHitBox(){
        meleeHitBox.SetActive(true);
    }

    public void DisableMeleeHitBox(){
        meleeHitBox.SetActive(false);
    }

    IEnumerator ResetHitBox(){
        meleeHitBox.SetActive(true);
        yield return new WaitForSeconds(hitBoxCooldown);
        meleeHitBox.SetActive(false);
    }

    public bool GetIsGuarding(){
        return isGuarding;
    }

    public void AddAttackDamage(float amount){
        attackDamage += amount;
    }
}
