using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PouceInput : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private Player player;
    [SerializeField] private InputAction inputAction;

    [Header("Tackle")]
    [SerializeField] private float isTacklingCD;
    [SerializeField] private float changeHeightDuration;
    private bool isTackling = false;
    private bool canTackle = true;

    [Header("Change Height")]
    [SerializeField] private float changeHeightCD;
    private float changeHeightDurationActual;
    private bool canChangeHeight = true;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = new Player();

        inputAction = player.Pouce.DPadRight;
        inputAction.Enable();

        inputAction.started += DPad_Right;
        inputAction.performed += DPad_Right;
        inputAction.canceled += DPad_Right;

        changeHeightDurationActual = 0;
    }

    private void Update()
    {
        ChangeHeight_Duration();
    }

    private void ChangeHeight_Duration()
    {
        if (canTackle)                                          changeHeightDurationActual += 0.01f;
        if (changeHeightDurationActual <= 0)                    return;
        if (!canTackle)                                         changeHeightDurationActual -= 0.01f;
        if (changeHeightDurationActual >= changeHeightDuration) StartCoroutine(ChangeHeightCD());
    }

    private void DPad_Right(InputAction.CallbackContext context)
    {
        if (context.started)    ChangeHeight(true);
        if (context.canceled)   ChangeHeight(false);
    }

    private void ChangeHeight(bool isChanging)
    {
        if (isChanging && canChangeHeight)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);
            canTackle = true;
        }  
        else
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
            canTackle = false;
        }
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
}
