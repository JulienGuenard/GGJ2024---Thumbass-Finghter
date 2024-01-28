using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionManager : MonoBehaviour
{
    public Fade fade;
    public List<Image> Fighters = new List<Image>(3);
    public Image P1Fighter;
    public Image P2Fighter;

    private int p1Selected;
    public int P1Selected
    {
        get => p1Selected;
        set
        {
            p1Selected = value % 3;

            if (value < 0)
                p1Selected = 2;
        }
    }

    private int p2Selected;
    public int P2Selected
    {
        get => p2Selected;
        set
        {
            p2Selected = value % 3;

            if (value < 0)
                p2Selected = 2;
        }
    }

    private int p1Locked;
    private int p2Locked;
    // Start is called before the first frame update
    void Awake()
    {
        fade.Out();

        p1Locked = -2;
        p2Locked = -2;
    }

    private void Start()
    {
        P1Fighter.color = Fighters[0].color;
        P2Fighter.color = Fighters[0].color;
    }

    bool waitForDrop1 = false;
    public void P1Input(InputAction.CallbackContext context)
    {
        if (p1Locked == -2)
        {
            if (context.ReadValue<Vector2>().y < -.6f && !waitForDrop1)
            {
                waitForDrop1 = true;

                P1Selected++;
                if (P1Selected == p2Locked)
                    P1Selected++;
            }
            if (context.ReadValue<Vector2>().y > .6f && !waitForDrop1)
            {
                waitForDrop1 = true;

                P1Selected--;
                if (P1Selected == p2Locked)
                    P1Selected--;
            }

            if (context.ReadValue<Vector2>().x < -.6f)
                p1Locked = P1Selected;

            P1Fighter.color = Fighters[P1Selected].color;
            Debug.Log(context.ReadValue<Vector2>().y);

            if (context.canceled)
                waitForDrop1 = false;
        }

        if (context.ReadValue<Vector2>().x > .6f)
            p1Locked = -2;
    }

    bool waitForDrop2 = false;
    public void P2Input(InputAction.CallbackContext context)
    {
        if (p2Locked == -2)
        {
            if (context.ReadValue<Vector2>().y > .6f && !waitForDrop2)
            {
                waitForDrop2 = true;

                P2Selected++;
                if (P2Selected == p1Locked)
                    P2Selected++;
            }
            if (context.ReadValue<Vector2>().y < -.6f && !waitForDrop2)
            {
                waitForDrop2 = true;

                P2Selected--;
                if (P2Selected == p1Locked)
                    P2Selected--;
            }

            if (context.ReadValue<Vector2>().x > .6f)
                p2Locked = P2Selected;

            P2Fighter.color = Fighters[P2Selected].color;
            Debug.Log(context.ReadValue<Vector2>().y);

            if (context.canceled)
                waitForDrop2 = false;
        }

        if (context.ReadValue<Vector2>().x < -.6f)
            p2Locked = -2;
    }
}
