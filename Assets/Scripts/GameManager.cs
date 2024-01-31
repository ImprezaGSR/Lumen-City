using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI skillDashMeterText;
    public Image skillDashMeterFill;
    public Image skillDashMeterIcon;
    public Image skillRightMeterFill;
    public Image skillRightMeterIcon;
    public Image reticleImage;
    public Image healthFill;
    public Color detectColor;
    private bool isPausing = false;
    private bool allowInteraction = true;

    bool gameHasEnded = false;
    public float restartDelay = 1f;

    void Start(){
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update(){
        if(allowInteraction){
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(!isPausing){
                    Time.timeScale = 0;
                    isPausing = true;
                    Cursor.lockState = CursorLockMode.None;
                }else{
                    Time.timeScale = 1;
                    isPausing = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
                allowInteraction = false;
                Debug.Log("Escape key was pressed");
            }
        }
        if(Input.GetKeyUp(KeyCode.Escape)){
            allowInteraction = true;
            Debug.Log("Escape key was released");
        }
    }

    public void EndGame(){
        if(gameHasEnded == false){
            gameHasEnded = true;
            Debug.Log("Game Over");
            Invoke("Restart", restartDelay);
        }
    }

    void Restart(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetHealthText(string text){
        healthText.text = text;
    }

    public void SetHealthFill(float amount, float amountMax){
        healthFill.fillAmount = amount/amountMax;
    }

    public void SetGoldText(string text){
        goldText.text = text;
    }

    public void SetDashMeterText(string text){
        skillDashMeterText.text = text;
    }

    public void SetSkillDashMeterFill(float amount, float amountMax){
        if(amount == amountMax){
            skillDashMeterFill.gameObject.SetActive(false);
            var tempColor = skillDashMeterIcon.color;
            tempColor.a = 1f;
            skillDashMeterIcon.color = tempColor;
        }else if(amount < amountMax){
            skillDashMeterFill.gameObject.SetActive(true);
            var tempColor = skillDashMeterIcon.color;
            tempColor.a = 0.25f;
            skillDashMeterIcon.color = tempColor;
        }
        skillDashMeterFill.fillAmount = amount/amountMax;
    }

    public void SetSkillRightMeterFill(float amount, float amountMax){
        if(amount == amountMax){
            skillRightMeterFill.gameObject.SetActive(false);
            var tempColor = skillRightMeterIcon.color;
            tempColor.a = 1f;
            skillRightMeterIcon.color = tempColor;
        }else if(amount < amountMax){
            skillRightMeterFill.gameObject.SetActive(true);
            var tempColor = skillRightMeterIcon.color;
            tempColor.a = 0.25f;
            skillRightMeterIcon.color = tempColor;
        }
        skillRightMeterFill.fillAmount = amount/amountMax;
    }


    public void SetReticleImageColor(bool detected){
        if(detected){
            reticleImage.color = detectColor;
        }else{
            reticleImage.color = Color.HSVToRGB(0,0,100);
        }
    }
}
