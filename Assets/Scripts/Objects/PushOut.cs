//PushOut made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/03
//usage:            push out anything contains in the collider / trigger.

using UnityEngine;
using System.Collections.Generic;

public class PushOut : STCMonoBehaviour
{
    private List<ObjectWithPosition> insideObjects = new List<ObjectWithPosition>();
    private ObjectWithPosition FindObjectInList(GameObject obj)
    {
        foreach (ObjectWithPosition objInList in insideObjects)
        {
            if (obj == objInList.gObject)
            {
                return objInList;
            }
        }
        return null;
    }

    public float pushOutSpeed = 10;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        if (pushOutSpeed < 0)
        {
            DebugMessage(LogType.Warning, "speed cannot less than 0. Will change to positive.");
            pushOutSpeed = Mathf.Abs(pushOutSpeed);
        }
        if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
        {
            DebugMessage(LogType.Error, "didn't contain any of collider/trigger. The script won't work.");
        }
    }

    private void ObjectEnter(GameObject obj, Vector3 enterPos)
    {
        insideObjects.Add(new ObjectWithPosition(obj, enterPos));
    }
    private void ObjectLeave(GameObject obj)
    {
        foreach (ObjectWithPosition objInList in insideObjects)
        {
            if (objInList.gObject == obj)
            {
                insideObjects.Remove(objInList);
                break;
            }
        }
    }
    private void Push(GameObject obj, Vector3 toPos, float timePassed)
    {
        obj.transform.position += (toPos - obj.transform.position) * timePassed;
    }

    private void OnCollisionEnter(Collision col)
    {
        ObjectEnter(col.gameObject, col.contacts[0].point);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        ObjectEnter(col.gameObject, col.contacts[0].point);
    }
    private void OnTriggerEnter(Collider col)
    {
        ObjectEnter(col.gameObject, col.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position));
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        ObjectEnter(col.gameObject, col.bounds.ClosestPoint(transform.position));
    }

    private void OnCollisionExit(Collision col)
    {
        ObjectLeave(col.gameObject);
    }
    private void OnCollisionExit2D(Collision2D col)
    {
        ObjectLeave(col.gameObject);
    }
    private void OnTriggerExit(Collider col)
    {
        ObjectLeave(col.gameObject);
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        ObjectLeave(col.gameObject);
    }

    private void OnCollisionStay(Collision col)
    {
        ObjectWithPosition target = FindObjectInList(col.gameObject);
        if (target != null)
        {
            Push(target.gObject, target.position, Time.fixedDeltaTime);
        }
    }
    private void OnCollisionStay2D(Collision2D col)
    {
        ObjectWithPosition target = FindObjectInList(col.gameObject);
        if (target != null)
        {
            Push(target.gObject, target.position, Time.fixedDeltaTime);
        }
    }
    private void OnTriggerStay(Collider col)
    {
        ObjectWithPosition target = FindObjectInList(col.gameObject);
        if (target != null)
        {
            Push(target.gObject, target.position, Time.fixedDeltaTime);
        }
    }
    private void OnTriggerStay2D(Collider2D col)
    {
        ObjectWithPosition target = FindObjectInList(col.gameObject);
        if (target != null)
        {
            Push(target.gObject, target.position, Time.fixedDeltaTime);
        }
    }

}

public class ObjectWithPosition
{
    public ObjectWithPosition()
    {
        gObject = null;
        position = Vector3.zero;
    }
    public ObjectWithPosition(GameObject go, Vector3 pos)
    {
        gObject = go;
        position = pos;
    }

    public GameObject gObject;
    public Vector3 position;

}
