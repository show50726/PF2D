using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : MonoBehaviour {

    private void OnDisable()
    {
        Debug.Log("On the start of OnDisable, enabled is " + enabled);
    }

}
