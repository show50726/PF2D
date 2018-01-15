//Finish made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/01/13
//usage:            this is a BASE class. Inherit this to make a custom one.

using UnityEngine;

public class Finish : MonoBehaviour
{
    [ReadOnly, Tooltip("Don't modifiy it, as it is used to system auto judge.")]
    public bool isFinished = false;
}
