using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExitTrigger : MonoBehaviour {

    [SerializeField]
    private UnityEvent EndGame;

    public void End()
    {
        GameSystemManager.exist.QuitGame();
        //this.EndGame.Invoke();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
            End();
    }


}
