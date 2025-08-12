using Unity.Entities;
using UnityEngine;

public class CollisionEventPhysicsTagAuthoring: MonoBehaviour {
 
    class Baker : Baker<CollisionEventPhysicsTagAuthoring> {
        public override void Bake(CollisionEventPhysicsTagAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new CollisionEventPhysicsTag());
        }
    }

}

public struct CollisionEventPhysicsTag : IComponentData
{
  
}
