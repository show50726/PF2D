using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour {

    public float checkSpeed = 9.8f;
    private Rigidbody2D rb2d;
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
                Debug.Log(name + " speed is " + rb2d.velocity.x);
            }
            if (rb2d.velocity.x >= checkSpeed || rb2d.velocity.x <= -checkSpeed)
            {
                Debug.Log(name + " speed has reached " + checkSpeed + "!");
            }
        }
    }
    
}
