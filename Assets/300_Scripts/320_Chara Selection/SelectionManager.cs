using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    // Start is called before the first frame update
    void Awake()
    {
        fade.Out();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            P1Right();
            Debug.Log($"P1 : New value = {P1Selected}");
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            P1Left();
            Debug.Log($"P1 : New value = {P1Selected}");
        }
        
        P1Fighter.color = Fighters[P1Selected].color;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            P2Right();
            Debug.Log($"P2 : New value = {P2Selected}");
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            P2Left();
            Debug.Log($"P2 : New value = {P2Selected}");
        }

        P2Fighter.color = Fighters[P2Selected].color;
    }

    public void P1Right()
    {
        P1Selected++;
    }

    public void P1Left()
    {
        P1Selected--;
    }

    public void P2Right()
    {
        P2Selected++;
    }

    public void P2Left()
    {
        P2Selected--;
    }
}
