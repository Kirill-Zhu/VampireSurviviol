using Unity.Entities;
using UnityEngine;

public class DestroyDelayedAuthoring : MonoBehaviour
{
    public float TimeToDestroy = 1;
    class Baker : Baker<DestroyDelayedAuthoring> {
        public override void Bake(DestroyDelayedAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new DestroyDelayed { TimeToDestroy = authoring.TimeToDestroy });

        }
    }
}
public struct DestroyDelayed: IComponentData {
    public float TimeToDestroy;
}