//PF (Platformer) 2D CameraController made by STC
//contact: stc.ntu@gmail.com
//last maintained: 2018/04/14
//NOTE: 2D only.
//Usage: add it to MainCamera, and this will work with other "PF-" scripts.
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PF2DCameraController : MonoBehaviour
{
    //private Camera cam;
    
    public GameObject[] player;

    [Header("Switch Setting")]
    private int playerIndex = 0;
    public KeyCode switchNext = KeyCode.O;
    public KeyCode switchBack;

    private Vector3 offset;         //Private variable to store the offset distance between the player and camera

    void Start()
    {
        //cam = GetComponent<Camera>();
        //Calculate and store the offset value by getting the distance between the player's position and camera's position.
        if (player.Length == 0)
        {
            Debug.LogWarning(GetType().Name + " of " + name + " warning: player is not assigned. The script won't work.");
            enabled = false;
        }
        if (player[0].GetComponent<Renderer>().isVisible == false)
        {
            transform.position = player[0].transform.position + new Vector3(4, 4, -10);
        }
        offset = transform.position - player[0].transform.position;
    }

    // LateUpdate is called after Update each frame
    //void LateUpdate()
    void Update()
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        transform.position = player[playerIndex].transform.position + offset;
        if (Input.GetKeyDown(switchNext))
        {
            playerIndex++;
            if (playerIndex >= player.Length) playerIndex = 0;
        }
        if (Input.GetKeyDown(switchBack))
        {
            playerIndex--;
            if (playerIndex < 0) playerIndex = player.Length - 1;
        }
    }
}
