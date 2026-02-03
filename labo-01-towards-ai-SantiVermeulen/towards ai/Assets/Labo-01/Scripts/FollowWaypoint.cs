using UnityEngine;

public class FollowWaypoint : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5.0f;
    public float rotSpeed = 4.0f;
    private int currentWaypoint = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 direction = waypoints[currentWaypoint].position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        // Hier is de code dat werd vermeld in de pdf "Smooth Rotation" de benaming zegt al wat het doet.
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotSpeed);

        transform.Translate(0, 0, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 2.0f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }
        }
    }
}
