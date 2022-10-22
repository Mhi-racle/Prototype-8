using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{

    public List<GameObject> roads;
    public float offset;
    void Start()
    {
        if(roads != null && roads.Count > 0)
        {
            roads = roads.OrderBy(r => r.transform.position.z).ToList();
        }
    }

    public void MoveRoad()
    {
        GameObject road = roads[0] ;
        roads.Remove(road);
        float newZ = roads[roads.Count - 1].transform.position.z + offset;
        road.transform.position = new Vector3(3.597794f, 5.176996f, newZ);
        roads.Add(road);
    }
}
