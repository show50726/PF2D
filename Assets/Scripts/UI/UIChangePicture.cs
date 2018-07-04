//UIChangePicture made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/04
//usage:            works with PropertyManager. It changes the specified picture as the UIManager provided keyword.

using UnityEngine;
using UnityEngine.UI;

public class UIChangePicture : STCMonoBehaviour
{
    public Image image;

    private void OnEnable()
    {
        if (image == null)
        {
            image = GetComponent<Image>();
            if (image == null)
            {
                DebugMessage(LogType.Error, "cannot find image. Did you forget to add it?");
                enabled = false;
                return;
            }
        }
    }
    public void ChangePicture(string pictureName)
    {

    }
    
}
