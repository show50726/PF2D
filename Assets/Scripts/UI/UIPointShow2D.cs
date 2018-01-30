//UIPointShow2D      made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/30
//usage:            this can be used as point show, such as HP, MP, etc.

using UnityEngine;
using UnityEngine.UI;

public class UIPointShow2D : MonoBehaviour {


    public Text pointShowText;
    [Tooltip("Should be full length. The script change its x scale to adjust the bar length.")]
    public GameObject bar;
    
    private double fullPoint = 100;

    private float fullLengthScale;
    private Vector3 leftLocation;
    private float originalLength = 0;
    private float fixScaleParameter;

    private void OnEnable()
    {
        if (bar)
        {
            fullLengthScale = bar.transform.localScale.x;
            Renderer rdr = bar.GetComponent<Renderer>();
            if (rdr)
            {
                fixScaleParameter = bar.transform.lossyScale.x / fullLengthScale;
                originalLength = rdr.bounds.size.x / fixScaleParameter;
                //DEV NOTE: the scale/size in unity is so fucked up, this set is the result after trying error.
                leftLocation =
                    bar.transform.localPosition - new Vector3(originalLength / 2, 0);
                /*
                Debug.Log("The left position of " + bar.name + " is " + leftLocation + "; "
                    + "the length of it is " + originalLength + "; " 
                    + "the absolute scale is " + bar.transform.lossyScale.x + "; "
                    + "the local scale is " + bar.transform.localScale.x);
                */
            }
            else
            {
                Debug.Log(GetType().Name + " reporting: the bar assigned doesn't have a renderer. Thus the point bar might not be aligned to left,  instead it will shrink to the center (or to the anchor if in Canvas).");
            }
        }
        else if (pointShowText == null)
        {
            Debug.LogWarning(GetType().Name + " warning: the bar and pointShowText are both not assigned. Script won't work, and probably will cause bugs.");
            enabled = false;
        }

    }

    public void SetFullPoint(double point)
    {
        fullPoint = point;
    }

    public void UpdatePoint(double pointNow)
    {
        float newScale = pointNow > 0 ? (float)(pointNow / fullPoint) : 0.01f;

        
        if (originalLength != 0)
        {
            
            bar.transform.localPosition = 
                leftLocation + new Vector3(originalLength * newScale / 2 , 0);
            bar.transform.localScale = new Vector3
                (fullLengthScale * newScale, bar.transform.localScale.y);
        }
        else if (bar)
        {
            bar.transform.localScale = new Vector3
                (fullLengthScale * newScale, bar.transform.localScale.y);
        }


        if (pointShowText)
        {
            string spacing = "";
            int align = fullPoint.ToString().Length - pointNow.ToString().Length;
            if (align > 0)
            {
                for (int i = 0; i < align; i++)
                {
                    spacing += "  ";
                }
            }

            pointShowText.text = spacing + pointNow + " / " + fullPoint;
        }

    }



}
