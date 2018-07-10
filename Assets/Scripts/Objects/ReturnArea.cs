//ReturnArea made by STC
//contact:          stc.ntu@gmail.com
//last maintained:  2018/07/10
//usage:            in some game -- like platformer -- in case the player go "too far," use this surround the level and the player would return.

using UnityEngine;
using UnityExtension;

public class ReturnArea : STCMonoBehaviour
{
    public LayerMask _IgnoreTheseLayers = (1 << 1) | (1 << 2) | (1 << 4) | (1 << 5);
    [Tooltip("portal anything who touched this to this point.")]
    public Vector3 destination;

    [Tooltip("If true, will zero velocity if there's rigidbody attached on objects.")]
    public bool stopRigidbody = true;

    private void OnEnable()
    {
        if (GetComponent<Collider>() == null && GetComponent<Collider2D>() == null)
        {
            DebugMessage(LogType.Error, "no trigger is set. Component will not work.");
            enabled = false;
            return;
        }
    }
    private void OnCollisionEnter(Collision col)
    {
        TryPortal(col.gameObject, destination);
    }
    private void OnCollisionEnter2D(Collision2D col)
    {
        TryPortal(col.gameObject, destination);
    }
    private void OnTriggerEnter(Collider col)
    {
        TryPortal(col.gameObject, destination);
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        TryPortal(col.gameObject, destination);
    }

    private void TryPortal(GameObject target, Vector3 dest)
    {
        if (!LayerMaskExtensions.IsInLayerMask(target, _IgnoreTheseLayers))
        {
            PortalTo(target, dest);
            if (stopRigidbody) SetRigidbodySpeed(target, Vector3.zero);
        }
    }

    private void PortalTo(GameObject target, Vector3 dest)
    {
        target.transform.position = dest;
    }

    private void SetRigidbodySpeed(GameObject target, Vector3 speed)
    {
        Rigidbody rb = target.GetComponent<Rigidbody>();
        Rigidbody2D rb2d = target.GetComponent<Rigidbody2D>();
        if (rb == null && rb2d == null) return;
        if (rb != null) rb.velocity = speed;
        if (rb2d != null) rb2d.velocity = speed;

    }



}
