//UIShowSceneName made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/04/14
//usage:            use it to update a UI.Text to scene name.
using UnityEngine;

public class UIShowSceneName : MonoBehaviour
{
    public UnityEngine.UI.Text showSceneName;
    public string formerDisplay = "[";
    public string laterDisplay = "]";

    private string sceneName = "";
    public void RefreshDisplay()
    {
        sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (showSceneName == null)
        {
            Debug.LogWarning(GetType().Name + " warning: didn't assign showSceneName. If you want to know the name of scene, it's \"" + sceneName + "\"");
        }
        showSceneName.text = formerDisplay + sceneName + laterDisplay;
    }

    private void Awake()
    {
        RefreshDisplay();
    }

}
