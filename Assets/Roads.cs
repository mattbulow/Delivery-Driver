using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Roads : MonoBehaviour
{
    private List<Collider2D> roadColliders;
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject boostPrefab;

    // Start is called before the first frame update
    void Start()
    {
        roadColliders = new List<Collider2D>();
        // create polygon colliders on all roads (all children)
        var children = this.GetComponentsInChildren<Transform>().Skip(1);
        //Debug.Log("_children size: " + _children.Length);

         foreach (Transform child in children)
        {
            if (child.gameObject == this.gameObject) { continue; }
            PolygonCollider2D poly = child.gameObject.AddComponent<PolygonCollider2D>();
            poly.isTrigger = true;
            roadColliders.Add(poly);
        }
    }

    void PlaceComponent(ref GameObject go)
    {
        // instantiate object at a random location within a random road object
        int roadIdx = UnityEngine.Random.Range(0, roadColliders.Count);
        Vector2 rndPoint2D = RandomPointInBounds2D(roadColliders[roadIdx].bounds);
        Vector2 rndPoint2DInside = roadColliders[roadIdx].ClosestPoint(rndPoint2D);
        Instantiate(go, new Vector3(rndPoint2DInside.x, rndPoint2DInside.y), Quaternion.identity);
    }

    Vector2 RandomPointInBounds2D(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            PlaceComponent(ref boostPrefab);
        }
    }






}

