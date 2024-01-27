using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PouceInput : MonoBehaviour
{
    [SerializeField] private InputAction inputAction_DPad;

    private bool isTackling = false;
    private bool canChangeHeight;
    private bool canTackle = true;

    [SerializeField] private float isTacklingCD;
    [SerializeField] private float changeHeightDuration;
    private float changeHeightDurationActual;
    [SerializeField] private float changeHeightCD;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        changeHeightDurationActual = 0;
    }

    void Update()
    {
        inputAction_DPad.Enable();
        inputAction_DPad.started += context => Debug.Log($"{context.action} started");
        inputAction_DPad.performed += context => Debug.Log($"{context.action} performed");
        inputAction_DPad.canceled += context => Debug.Log($"{context.action} canceled");

        return;
        if (Input.GetAxis("DPad-Horizontal") > 0)   Tackle();
        if (Input.GetAxis("Horizontal") > 0)        Rotate();
        if (Input.GetAxis("DPad-Vertical") > 0)     ChangeHeight(true);
        else                                        ChangeHeight(false);
    }

    void Tackle()
    {
        if (!canTackle) return;

        isTackling = true;
        animator.SetBool("isTackling", true);
        StartCoroutine(TackleCD());
    }

    void Rotate()
    {

    }

    void ChangeHeight(bool isChanging)
    {
       // Debug.Log(Input.GetAxis("DPad-Vertical"));

        if (isChanging && canChangeHeight)
        {
            transform.position = new Vector3(transform.position.x, 1, 0);
            changeHeightDurationActual += 0.01f;
            canTackle = true;
        }  
        else
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
            changeHeightDurationActual -= 0.01f;
            canTackle = false;
        }

        if (changeHeightDurationActual >= changeHeightDuration) StartCoroutine(ChangeHeightCD());
    }

    IEnumerator TackleCD()
    {
        yield return new WaitForSeconds(isTacklingCD);
        isTackling = false;
        animator.SetBool("isTackling", false);
    }

    IEnumerator ChangeHeightCD()
    {
        changeHeightDurationActual = 0;
        canChangeHeight = false;
        yield return new WaitForSeconds(changeHeightCD);
        canChangeHeight = true;
    }
}
