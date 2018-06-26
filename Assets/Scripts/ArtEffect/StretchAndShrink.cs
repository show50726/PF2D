//StretchAndShrink made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/25
//Usage:            Assign it to the object, then the object will stretch & shrink (by Y scale).
using UnityEngine;

public class StretchAndShrink : MonoBehaviour
{

    public float minScale = 0;
    public float maxScale = 1.5f;
    [Tooltip("Per second.")]
    public float changingSpeed = 0.2f;

    [Tooltip("Initial direction. Will auto change while executing.")]
    public bool goBigger = true;

    [Tooltip("Assign it to skip height calculation.")]
    public float assignHeight = 0;

    /// <summary>
    /// originHeight means the height when object is scale 1, in the SAME Parent.
    /// </summary>
    private float originHeight = 1;
    private string debugTag;

    // Use this for initialization
    void Start()
    {
        debugTag = name + "/" + GetType().Name;
        Renderer rend = GetComponent<Renderer>();
        if (assignHeight > 0)
        {
            originHeight = assignHeight;
        }
        else if (rend == null)
        {
            Debug.LogWarning(debugTag + " warning: cannot find renderer. The position might be wrong.");
            originHeight = 1 / transform.localScale.y;
        }
        else
        {
            originHeight = rend.bounds.size.y / transform.localScale.y;
        }
        //Debug.Log(debugTag + ": localscale is " + transform.localScale.y + ", lossy is " + transform.lossyScale.y + ", bounds is" + rend.bounds.size.y);
    }

    // Update is called once per frame
    void Update()
    {
        ScaleStretch();
    }
    

    private void ScaleStretch()
    {
        float delY = changingSpeed * Time.deltaTime;
        if (goBigger)
        {
            //go bigger
            if (transform.localScale.y + delY >= maxScale)
            {
                delY = maxScale - transform.localScale.y;
                goBigger = false;
            }
        }
        else
        {
            //go smaller
            if (transform.localScale.y - delY <= minScale)
            {
                delY = minScale - transform.localScale.y;
                goBigger = true;
            }
            else
            {
                delY = - delY;
            }
        }
        //do action: stretch & reset position
        transform.localScale += new Vector3(0, delY);
        transform.position += new Vector3(0, delY * originHeight / 2);
    }
}
