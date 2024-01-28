using System;
using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PouceP2 : MonoBehaviour
{
    #region Variables
    public GameObject OtherThumb;

    public GameObject victoryGMB;

    [Header("Tackle")]
    [SerializeField] private float isTacklingCD;
    [SerializeField] private float changeHeightDuration;
    private bool canTackle = true;

    [Header("Change Height")]
    [SerializeField] private float changeHeightCD;
    private float changeHeightDurationActual;
    private bool canChangeHeight = true;

    [HideInInspector]
    public Animator animator;
    private Rigidbody rb;
    //Vector3 rbRot;

    //bool lockRot = false;
    #endregion

    #region Init Methods

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody>();

        changeHeightDurationActual = 0;

        //lockRot = false;
    }

    //private void Start()
    //{
    //    rb.rotation = Quaternion.Euler(0, rb.rotation.y - 10, rb.rotation.z + 10);
    //}

    private void Update()
    {
        ChangeHeight_Duration();

        //if (lockRot)
        //{
        //    rbRot.y = Mathf.Clamp(rbRot.y, rb.rotation.y, 190);
        //    rb.rotation = Quaternion.Euler(rb.rotation.x, rbRot.y, rb.rotation.z);
        //}
    }
    #endregion

    #region Input Methods
    public void Btn_X(InputAction.CallbackContext context)
    {
        if (context.started)
            ChangeHeight(true);

        if (context.canceled)
            ChangeHeight(false);
    }

    public void Btn_RT(InputAction.CallbackContext context)
    {
        if (animator.GetBool("isElevated") == true)
        {
            animator.SetBool("isElevated", false);
            animator.SetBool("isTackling", true);
            StartCoroutine(TackleCD());
        }
    }

    public void Joystick_Right(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        rb.rotation = Quaternion.Euler(0, -180 + input.y * 10, input.y * -10);

        if (context.canceled)
            rb.rotation = Quaternion.Euler(0, -180, 0);
    }
    #endregion

    #region Gameplay Methods
    private void ChangeHeight(bool isChanging)
    {
        if (isChanging && canChangeHeight)
        {
            rb.position = new Vector3(rb.position.x, 1, rb.position.z);
            animator.SetBool("isElevated", true);
            canTackle = true;
        }
        else
        {
            rb.position = new Vector3(rb.position.x, 0, rb.position.z);
            animator.SetBool("isElevated", false);
            canTackle = false;
        }
    }

    private void ChangeHeight_Duration()
    {
        if (canTackle)
            changeHeightDurationActual += 0.01f;

        if (!canTackle)
            changeHeightDurationActual -= 0.01f;

        if (changeHeightDurationActual >= changeHeightDuration)
            StartCoroutine(ChangeHeightCD());
    }

    private IEnumerator TackleCD()
    {
        yield return new WaitForSeconds(isTacklingCD);
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
    #endregion

    #region CollisionHandler
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == OtherThumb)
        {
            if (OtherThumb.TryGetComponent(out PouceP1 p))
            {
                if (animator.GetBool("isTackling") && !p.animator.GetBool("isTackling"))
                {
                    Debug.Log("Right Thumb wins !");
                    victoryGMB.SetActive(true);
                    StartCoroutine(RestartDelay());
                }
            }
        }
    }

    IEnumerator RestartDelay()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    #endregion
}
