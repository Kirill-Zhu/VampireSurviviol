using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistributeObjects : MonoBehaviour
{
    public Text Text;
    public GameObject targetObject;
    public float Interval = 1;
    public float ZoneRadius = 10;
    public float ObjectRadius = 0.25f;
    private List<Vector2> positions = new List<Vector2>();
    private float time = 0;

    private void Awake()
    {
        positions.Clear();
    }
    
    private void Update()
    {
        time += Time.deltaTime;
        if (time > Interval)
        {
            time -= Interval;
            GenerateNewObject();
        }
    }

    private void GenerateNewObject()
    {
        //var k = 1000;
        //while (k > 0)
        {
            //k--;
            var newPoint = Random.insideUnitCircle * ZoneRadius;
            // var isInObjectRadius = false;
            // foreach (var position in positions)
            // {
            //     if (Vector2.Distance(position, newPoint) > ObjectRadius) continue;
            //     isInObjectRadius = true;
            //     break;
            // }
            
            //if (isInObjectRadius) continue;

            var newObject = Instantiate(targetObject, transform);
            newObject.transform.localPosition = new Vector3(newPoint.x, 0, newPoint.y);
            newObject.SetActive(true);
            positions.Add(newPoint);
            //break;
        }

        Text.text = positions.Count.ToString();
    }
}