using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class CollisionEventDataAuthoring: MonoBehaviour {
 
    public float Impulse;
    public int Damage;
    class Baker : Baker<CollisionEventDataAuthoring> {
        public override void Bake(CollisionEventDataAuthoring authoring) {
          Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new CollisionEventData { Impulse = authoring.Impulse,
             Damage = authoring.Damage});
        }
    }
}
public struct CollisionEventData : IComponentData
{
    public Entity EntityA;
    public Entity EntityB;
    public float3 ImpactNormal;
    public float3 Position;
    public float Impulse;
    public int Damage;

}
public struct ProcessCollisionTag : IComponentData { }