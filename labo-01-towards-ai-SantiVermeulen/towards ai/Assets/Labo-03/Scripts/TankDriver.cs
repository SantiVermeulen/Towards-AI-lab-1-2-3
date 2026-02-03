using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class TankDriver : MonoBehaviour
{
    public WPManager wpManager;
    public TMP_Dropdown dropdown;
    public float speed = 5.0f;
    public float rotSpeed = 4.0f;
    public float accuracy = 1.0f;

    private Graph graph;
    private GameObject currentNode;
    private int pathIndex = 0;

    void Start()
    // initialisatie van de dropdown en het graph
    {
        graph = wpManager.graph;

        dropdown.ClearOptions();
        List<string> options = new List<string>();
        foreach (GameObject wp in wpManager.waypoints)
        {
            options.Add(wp.name);
        }
        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(delegate { DriveToDestination(); });
    }

    public void DriveToDestination()
    // berekenen van het pad met A*
    {
        GameObject startNode = FindClosestWaypoint();
        GameObject endNode = wpManager.waypoints[dropdown.value];

        if (graph.AStar(startNode, endNode))
        {
            pathIndex = 0;
        }
    }

    void Update()
        // beweging van de tank
    {
        if (graph.pathList.Count == 0 || pathIndex >= graph.pathList.Count)
        {
            return;
        }
        currentNode = graph.getPathPoint(pathIndex);
        Vector3 targetPosition = currentNode.transform.position;

        Vector3 direction = targetPosition - transform.position;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
        transform.Translate(0, 0, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPosition) < accuracy)
        {
            pathIndex++;
        }
    }

    GameObject FindClosestWaypoint()
    {
        GameObject closest = null;
        float minDistance = float.MaxValue;
        foreach (GameObject wp in wpManager.waypoints)
        {
            float dist = Vector3.Distance(transform.position, wp.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = wp;
            }
        }
        return closest;
    }
}