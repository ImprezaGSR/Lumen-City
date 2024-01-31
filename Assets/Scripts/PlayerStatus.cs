using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    private GameManager gameManager;

    public string characterName;

    public float healthMax = 100;
    [SerializeField]
    float health;

    public bool isInvincible = false;
    public float invincibleTime = 0.25f;

    public float skillRightMeterMax = 3f;
    public float skillRightMeter;

    public float skillDashMeterMax = 5f;
    public float skillDashMeter;

    public float skillSpecialMeterMax = 30f;
    public float skillSpecialMeter;

    

    public int gold;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        health = healthMax;
        skillDashMeter = skillDashMeterMax;
        skillRightMeter = skillRightMeterMax;
        gameManager.SetHealthText("Health: "+health+"/"+healthMax);
        gameManager.SetHealthFill(health,healthMax);
        gameManager.SetGoldText("Gold: "+gold);
        // gameManager.SetDashMeterText("SkillDash: "+skillDashMeter+"/"+skillDashMeterMax);
        gameManager.SetSkillDashMeterFill(skillDashMeter,skillDashMeterMax);
    }

    // Update is called once per frame
    void Update()
    {
        if(skillDashMeter < skillDashMeterMax){
            skillDashMeter += Time.deltaTime;
        }else{
            skillDashMeter = skillDashMeterMax;
        }

        if(skillRightMeter < skillRightMeterMax){
            skillRightMeter += Time.deltaTime;
        }else{
            skillRightMeter = skillRightMeterMax;
        }

        gameManager.SetSkillDashMeterFill(skillDashMeter,skillDashMeterMax);
        gameManager.SetSkillRightMeterFill(skillRightMeter,skillRightMeterMax);
    }

    public void Damage(float amount){
        if(!isInvincible){
            health -= amount;
            if(health <= 0){
                health = 0;
                gameManager.EndGame();
            }
            gameManager.SetHealthText("Health: "+health+"/"+healthMax);
            gameManager.SetHealthFill(health,healthMax);
            StartCoroutine(ResetInvincibleTime());
        }
    }

    public void Heal(float amount){
        health += amount;
        if(health > healthMax){
            health = healthMax;
        }
        gameManager.SetHealthText("Health: "+health+"/"+healthMax);
        gameManager.SetHealthFill(health,healthMax);
    }

    public void AddGold(int amount){
        gold += amount;
        gameManager.SetGoldText("Gold: "+gold);
    }

    public void SpendGold(int amount){
        gold -= amount;
        gameManager.SetGoldText("Gold: "+gold);
    }

    IEnumerator ResetInvincibleTime(){
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }
}
