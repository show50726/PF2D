using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour {

    public float checkSpeed = 9.8f;
    private Rigidbody2D rb2d;

    public KeyCode activateKey = KeyCode.L;
    public Mechanism2D[] activateThis = new Mechanism2D[1];
    public Vector2 force = Vector2.zero;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (rb2d)
        {
            if (rb2d.velocity.x != 0)
            {
                //Debug.Log(name + " speed is " + rb2d.velocity.x);
            }
            if (rb2d.velocity.x >= checkSpeed || rb2d.velocity.x <= -checkSpeed)
            {
                //Debug.Log(name + " speed has reached " + checkSpeed + "!");
            }
            if (rb2d.velocity.y >= checkSpeed || rb2d.velocity.y <= -checkSpeed)
            {
                //Debug.Log(name + " jump speed has reached " + checkSpeed + "!");
            }
        }
        if (Input.GetKeyDown(activateKey))
        {
            foreach (Mechanism2D m in activateThis)
            {
                m.Activated = !m.Activated;
            }
        }

    }
    private void OnMouseDown()
    {
        rb2d.AddForce(force);
        Debug.Log(force + "force added!!!");
    }

}
