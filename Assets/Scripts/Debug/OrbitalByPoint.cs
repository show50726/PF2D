using UnityEngine;
using System.Collections;

public class OrbitalByPoint : MonoBehaviour
{
    public Transform center;
    public float radius = 10;
    [Tooltip("Enable this will replace radius with initial distance between these 2 objects.")]
    public bool _UseDistanceOnStart = false;
    public float startAngle = 0;
    [Tooltip("Enable this will replace startAngle with initial direction.")]
    public bool _UseAngleOnStart = false;
    private float angle = 0;
    [Tooltip("If you want to rotate CCW, use positive; use negative value for CW otherwise.")]
    public float angPerSec = 120;

    void OnEnable()
    {
        if (center == null)
        {
            Debug.LogError(gameObject.name +"/" + GetType().Name + " error: transform not assigned.");
            enabled = false;
            return;
        }
        if (angPerSec == 0)
        {
            Debug.LogWarning(gameObject.name + "/" + GetType().Name + " warning: angular speed set to 0.");
        }
        if (_UseDistanceOnStart)
        {
            radius = Vector3.Distance(center.position, transform.position);
        }
        if (_UseAngleOnStart)
        {
            //startAngle = Vector3.Angle(Vector3.right, transform.position - center.position);
            Vector3 direction = transform.position - center.position;
            startAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        }
        angle = startAngle;
        transform.position = center.position + GetVectorByAngleAndRadius(radius, angle);
    }

    // Update is called once per frame
    void Update()
    {
        angle += angPerSec * Time.deltaTime;
        angle %= 360;
        transform.position = center.position + GetVectorByAngleAndRadius(radius, angle);
    }
    public Vector3 GetVectorByAngleAndRadius(float radius, float angle)
    {
        return new Vector3(
            radius * Mathf.Cos(angle * Mathf.Deg2Rad),
            radius * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
    }

}
