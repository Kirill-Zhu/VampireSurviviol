using Unity.Entities;
using UnityEngine;

public class PauseComponentAuthoring : MonoBehaviour
{
    [SerializeField] bool IsPaused = false;
    class Baker : Baker<PauseComponentAuthoring> {
        public override void Bake(PauseComponentAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PauseComponent { IsPaused = authoring.IsPaused});
        }
    }
}
public struct PauseComponent : IComponentData {
    public bool IsPaused;
}