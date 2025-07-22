using Unity.Entities;
using UnityEngine;

public class CollisionDetectionauthoring : MonoBehaviour
{
    class Baker: Baker<CollisionDetectionauthoring> {
        
public override void Bake(CollisionDetectionauthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<CollisionProcessor>(entity);   
        }
    }
}


