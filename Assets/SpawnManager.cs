using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    private List<Collider2D> roadColliders;
    [SerializeField] private GameObject roadsParentObject;
    [SerializeField] private GameObject packagePrefab;
    [SerializeField] private GameObject customerPrefab;
    [SerializeField] private GameObject boostPrefab;

    [SerializeField] private Color[] packageColors = { Color.red, Color.blue, Color.green, Color.yellow};

    // Start is called before the first frame update
    void Start()
    {

        roadColliders = new List<Collider2D>();
        // create polygon colliders on all roads (all children)
        var children = roadsParentObject.GetComponentsInChildren<Transform>().Skip(1);
        foreach (Transform child in children)
        {
            PolygonCollider2D poly = child.gameObject.AddComponent<PolygonCollider2D>();
            poly.isTrigger = true;
            roadColliders.Add(poly);
        }

        // start game by placing 3 sets of package/customers and 2 boosts
        int numberOfPackages = 4;
        int numberOfBoosts = 2;
        for (int n = 0; n < numberOfPackages; n++)
        {
            Color color;
            if (n >= packageColors.Length)
            {
                //public static Color ColorHSV(float hueMin, float hueMax, float saturationMin, float saturationMax, float valueMin, float valueMax, float alphaMin, float alphaMax);
                color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            } else
            {
                color = packageColors[n];
            }
            PlaceComponent(ref packagePrefab, color);
            PlaceComponent(ref customerPrefab, color);
            if (n < numberOfBoosts) { PlaceComponent(ref boostPrefab, new Color32(255,0,255,255)); }
        }

    }

    void PlaceComponent(ref GameObject go, Color color)
    {
        // instantiate object at a random location within a random road object
        int roadIdx = UnityEngine.Random.Range(0, roadColliders.Count);
        Vector2 rndPoint2D = RandomPointInBounds2D(roadColliders[roadIdx].bounds);
        Vector2 rndPoint2DInside = roadColliders[roadIdx].ClosestPoint(rndPoint2D);
        GameObject newObj = Instantiate(go, new Vector3(rndPoint2DInside.x, rndPoint2DInside.y), Quaternion.identity,this.transform);
        newObj.GetComponent<SpriteRenderer>().color = color;
    }
    Vector2 RandomPointInBounds2D(Bounds bounds)
    {
        return new Vector2(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y));
    }


}
