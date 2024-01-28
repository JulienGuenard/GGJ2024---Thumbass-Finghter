using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PouceInput : MonoBehaviour
{
    #region Variables
    [Header("Input")]
    [SerializeField] private Player player;
    [SerializeField] private bool can_dPad_Right, can_joystickLeft, can_joystickRight, can_btn_LT, can_btn_X, can_btn_RT;
    private InputAction dPad_Right, joystickLeft, joystickRight, btn_LT, btn_X, btn_RT;

    [Header("Tackle")]
    [SerializeField] private float isTacklingCD;
    [SerializeField] private float changeHeightDuration;
    private bool isTackling = false;
    private bool canTackle = true;

    [Header("Change Height")]
    [SerializeField] private float changeHeightCD;
    private float changeHeightDurationActual;
    private bool canChangeHeight = true;

    [Header("Rotation")]
    [SerializeField] private bool isInverted;
    [SerializeField] private float rotationSpeed;

    bool rotBlockLeft = false;
    bool rotBlockRight = false;

    private Animator animator;
    private Rigidbody rigidbody;
    #endregion

    #region Init Methods

    public delegate void TestDelegate();

    private void Awake()
    {
        Awake_Vars();
        Awake_Input();
    }

    void Awake_Vars()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        player = new Player();

        changeHeightDurationActual = 0;
    }

    void Awake_Input()
    {
        // Player 1
        dPad_Right = player.Pouce.DPadRight;
        dPad_Right.Enable();
        dPad_Right.started += DPad_Right;
        dPad_Right.performed += DPad_Right;
        dPad_Right.canceled += DPad_Right;

        joystickLeft = player.Pouce.JoystickLeft;
        joystickLeft.Enable();
        joystickLeft.started += Joystick_Left;
        joystickLeft.performed += Joystick_Left;
        joystickLeft.canceled += Joystick_Left;

        btn_LT = player.Pouce.LT;
        btn_LT.Enable();
        btn_LT.started += Btn_LT;
        btn_LT.performed += Btn_LT;
        btn_LT.canceled += Btn_LT;

        // Player 2
        btn_X = player.Pouce.X;
        btn_X.Enable();
        btn_X.started += Btn_X;
        btn_X.performed += Btn_X;
        btn_X.canceled += Btn_X;

        joystickRight = player.Pouce.JoystickRight;
        joystickRight.Enable();
        joystickRight.started += Joystick_Right;
        joystickRight.performed += Joystick_Right;
        joystickRight.canceled += Joystick_Right;

        btn_RT = player.Pouce.RT;
        btn_RT.Enable();
        btn_RT.started += Btn_RT;
        btn_RT.performed += Btn_RT;
        btn_RT.canceled += Btn_RT;
    }

    private void Update()
    {
        ChangeHeight_Duration();
        RotationClamp();
    }
    #endregion

    #region Input Methods
    // Change Height
    private void DPad_Right(InputAction.CallbackContext context)
    {
        if (!can_dPad_Right) return;

        if (context.started)    ChangeHeight(true);
        if (context.canceled)   ChangeHeight(false);
    }

    private void Btn_X(InputAction.CallbackContext context)
    {
        if (!can_btn_X) return;

        if (context.started) ChangeHeight(true);
        if (context.canceled) ChangeHeight(false);
    }

    // Tackle
    private void Btn_LT(InputAction.CallbackContext context)
    {
        if (!can_btn_LT) return;
    }

    private void Btn_RT(InputAction.CallbackContext context)
    {
        if (!can_btn_RT) return;
    }

    // Rotate
    private void Joystick_Left(InputAction.CallbackContext context)
    {
        if (!can_joystickLeft) return;

        Vector2 input = context.ReadValue<Vector2>();

        int inversion = 1;

        if (isInverted) inversion = -1;

        if (context.canceled) { rigidbody.angularVelocity = Vector3.zero; return; }
        if (input.y > 0.1f && rotBlockLeft) { rigidbody.angularVelocity = Vector3.zero; return; }
        if (input.y < 0.1f && rotBlockRight) { rigidbody.angularVelocity = Vector3.zero; return; }

        rigidbody.AddRelativeTorque(0, 0, input.y * rotationSpeed);
    }

    private void Joystick_Right(InputAction.CallbackContext context)
    {
        if (!can_joystickRight) return;

        Vector2 input = context.ReadValue<Vector2>();

        int inversion = 1;

        if (isInverted) inversion = -1;

        if (context.canceled) { rigidbody.angularVelocity = Vector3.zero; return; }
        if (input.y > 0.1f && rotBlockLeft) { rigidbody.angularVelocity = Vector3.zero; return; }
        if (input.y < 0.1f && rotBlockRight) { rigidbody.angularVelocity = Vector3.zero; return; }

        rigidbody.AddRelativeTorque(0, 0, input.y * rotationSpeed);

        if (context.canceled) rigidbody.angularVelocity = Vector3.zero;
    }

    #endregion

    #region Gameplay Methods
    private void ChangeHeight(bool isChanging)
    {
        if (isChanging && canChangeHeight)
        {
            transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            canTackle = true;
        }  
        else
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            canTackle = false;
        }
    }

    private void ChangeHeight_Duration()
    {
        if (canTackle) changeHeightDurationActual += 0.01f;
        if (changeHeightDurationActual <= 0) return;
        if (!canTackle) changeHeightDurationActual -= 0.01f;
        if (changeHeightDurationActual >= changeHeightDuration) StartCoroutine(ChangeHeightCD());
    }

    private IEnumerator TackleCD()
    {
        yield return new WaitForSeconds(isTacklingCD);
        isTackling = false;
        animator.SetBool("isTackling", false);
    }

    private IEnumerator ChangeHeightCD()
    {
        changeHeightDurationActual = 0;
        ChangeHeight(false);
        canChangeHeight = false;
        yield return new WaitForSeconds(changeHeightCD);
        canChangeHeight = true;
    }

    private void RotationClamp()
    {
        if (transform.rotation.eulerAngles.z > 45 && transform.rotation.eulerAngles.z < 150)    rotBlockLeft = true;
        else                                                                                    rotBlockLeft = false;

        if (transform.rotation.eulerAngles.z < 315 && transform.rotation.eulerAngles.z > 150)   rotBlockRight = true;
        else                                                                                    rotBlockRight = false;
    }
    #endregion
}
