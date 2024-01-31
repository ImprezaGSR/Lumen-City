using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;
    private PlayerInput.OnFootActions onFoot;
    private PlayerMotor motor;
    private PlayerLook look;
    private PlayerAttack attack;
    public ViewBobbing bob;
    // Start is called before the first frame update
    void Awake()
    {
        playerInput = new PlayerInput();
        onFoot = playerInput.OnFoot;
        motor = GetComponent<PlayerMotor>();
        look = GetComponent<PlayerLook>();
        attack = GetComponent<PlayerAttack>();
        //ctx = callback context
        onFoot.Jump.performed += ctx => motor.Jump();
        onFoot.Dash.performed += ctx => motor.Dash(onFoot.Movement.ReadValue<Vector2>());
        onFoot.Dash.performed += ctx => attack.AttackDash();
        onFoot.Dash.canceled += ctx => motor.StopDash();
        onFoot.Attack.performed += ctx => attack.Attack();
        onFoot.SkillRight.performed += ctx => attack.SkillRight();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Tell the playerMotor to move using the value from out movement action
        motor.ProcessMove(onFoot.Movement.ReadValue<Vector2>());
        bob.ProcessBobbing(onFoot.Movement.ReadValue<Vector2>());
    }

    void LateUpdate(){
        look.ProcessLook(onFoot.Look.ReadValue<Vector2>());
    }

    private void OnEnable()
    {
        onFoot.Enable();
    }

    private void OnDisable()
    {
        onFoot.Disable();
    }
}

