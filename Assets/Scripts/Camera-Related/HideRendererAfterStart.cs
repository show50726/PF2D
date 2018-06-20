//Hide Renderer After Start made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/05/23
//usage: sometimes you may want an object shown in editor, while not shown in game --- such as death area, AI view, etc, then this will come handy. It will close renderer after game start.

using UnityEngine;
using System.Collections.Generic;

public class HideRendererAfterStart : MonoBehaviour
{
    public new List<Renderer> renderer = new List<Renderer>();
    public KeyCode oneKeyToggle = KeyCode.Backslash;

    private void OnEnable()
    {
        int i = 0;
        while (i < renderer.Count)
        {
            if (renderer[i] == null)
            {
                renderer.RemoveAt(i);
            }
        }
        if (renderer.Count == 0)
        {
            Renderer r = GetComponent<Renderer>();
            if (r == null)
            {
                Debug.Log(name + "/" + GetType().Name + ": no renderer found. will do nothing and unable this component.");
                enabled = false;
                return;
            }
            renderer.Add(r);
        }
        ToggleRenderer();
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(oneKeyToggle))
        {
            ToggleRenderer();
        }
    }

    private void ToggleRenderer()
    {
        foreach (Renderer r in renderer)
        {
            if (r != null)
            {
                r.enabled = !r.enabled;
            }
        }
    }

}
