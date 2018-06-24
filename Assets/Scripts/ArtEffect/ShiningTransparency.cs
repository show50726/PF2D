using UnityEngine;
using System.Collections;
using System;

public class ShiningTransparency : MonoBehaviour
{
    [Range(0.002f, 0.9f)]
    [Tooltip("The transparency value in program verify from 0 to 1. if changing value too large, this will make color looks like the same.")]
    public float changingSpeed = 0.005f;
    [Range(0, 1)]
    public float leastTransparency = 0;
    [Range(0, 1)]
    public float maxTransparency = 1;
    private new SpriteRenderer renderer;
    private bool upward = true;

    private void OnEnable()
    {
        renderer = GetComponent<SpriteRenderer>();
        if (renderer == null)
        {
            Debug.LogWarning(name + "/" + GetType().Name + " warning: you didn't assign text, thus this script won't work.");
            enabled = false;
            return;
        }
        if (maxTransparency > leastTransparency)
        {
            Debug.LogWarning(name + "/" + GetType().Name + " warning: maxTransparency is less than leastTransparency. I assume that you mispelled them");
        }
    }

    private void Update()
    {
        TransparencyShine();
    }

    private void TransparencyShine()
    {
        if (upward)
        {
            //go valid
            if (renderer.color.a + changingSpeed >= maxTransparency)
            {
                renderer.color += new Color(0, 0, 0, maxTransparency - renderer.color.a);
                upward = false;
                return;
            }
            else
            {
                renderer.color += new Color(0, 0, 0, changingSpeed);
                return;
            }
        }
        else
        {
            //go transparent
            if (renderer.color.a - changingSpeed <= leastTransparency)
            {
                renderer.color += new Color(0, 0, 0, leastTransparency - renderer.color.a);
                upward = true;
                return;
            }
            else
            {
                renderer.color += new Color(0, 0, 0, - changingSpeed);
                return;
            }
        }

    }
}
