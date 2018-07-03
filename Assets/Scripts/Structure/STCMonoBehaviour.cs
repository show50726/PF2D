//STCMonoBehaviour  made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/06/27
//usage:            an expanded version of MonoBehaviour. Inherit this to get additional function.

using UnityEngine;

public class STCMonoBehaviour : MonoBehaviour
{
    public bool _showDebugMessage = false;
    protected string _debugTitle = "";
    protected virtual void Awake()
    {
        _debugTitle = name + "/" + GetType().Name;

    }
    

    public void DebugMessage(LogType type, string content)
    {
        switch (type)
        {
            case LogType.Normal:
                if(_showDebugMessage) Debug.Log(_debugTitle + ": " + content);
                break;
            case LogType.Warning:
                if (_showDebugMessage) Debug.LogWarning(_debugTitle + " warning: " + content);
                break;
            case LogType.Error:
                Debug.LogError(_debugTitle + " error: " + content);
                break;
            default:
                Debug.Log(_debugTitle + ": " + content);
                break;
        }
    }

}

public enum LogType { Normal, Warning, Error }
