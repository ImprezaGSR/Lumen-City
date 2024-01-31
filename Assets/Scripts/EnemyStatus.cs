using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStatus : MonoBehaviour
{
    public string enemyName;

    public float healthMax = 100;
    [SerializeField]
    float health;
    public TextMeshProUGUI healthText;
    public Image healthImage;
    
    public bool isInvincible = false;
    public float invincibleTime = 0.25f;

    public float attackDamage = 20;

    public int gold = 0;

    Collider collider;

    private EnemyMotor motor;

    private DetectEnemy detectEnemy;

    [SerializeField]
    private Renderer[] material;

    public bool isDissolving = false;

    [SerializeField]
    private float originCutoffHeight;
    
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
        motor = GetComponent<EnemyMotor>();
        detectEnemy = FindObjectOfType<DetectEnemy>().GetComponent<DetectEnemy>();
        health = healthMax;
        healthText.text = enemyName;
        healthImage.fillAmount = health/healthMax;
        originCutoffHeight = material[0].material.GetFloat("_CutoffHeight");
    }

    // Update is called once per frame
    void Update()
    {
        if(isDissolving){
            originCutoffHeight -= Time.deltaTime * 200;
            foreach(Renderer renderer in material){
                renderer.material.SetFloat("_CutoffHeight", originCutoffHeight);
            }
            if(originCutoffHeight < -70){
                Destroy(gameObject);
            }
        }
    }

    public void Damage(float amount){
        if(!isInvincible){
            health -= amount;
            if(health <= 0){
                health = 0;
                // detectEnemy.RemoveOnDestroy(this.gameObject);
                // Destroy(gameObject);
                motor.hasKilled = true;
                motor.Knockback(amount * 20);
                isDissolving = true;
                healthText.transform.parent.gameObject.SetActive(false);
            }
            // healthText.text = enemyName+": "+health+"/"+healthMax;
            healthImage.fillAmount = health/healthMax;
            StartCoroutine(ResetInvincibleTime());
            collider.isTrigger = true;
            motor.isStopping = true;
        }
    }

    public void Heal(float amount){
        health += amount;
        if(health > healthMax){
            health = healthMax;
        }
        healthImage.fillAmount = health/healthMax;
        // healthText.text = enemyName+": "+health+"/"+healthMax;
    }

    IEnumerator ResetInvincibleTime(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
        collider.isTrigger = false;
        motor.isStopping = false;
        motor.detectTarget = true;
    }
}
