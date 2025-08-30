using Unity.Entities;
using UnityEngine;

public class TakeUpGunTriggerAuthoring: MonoBehaviour {
    [SerializeField] private GunType _gunType;

    class Baker : Baker<TakeUpGunTriggerAuthoring> {
        public override void Bake(TakeUpGunTriggerAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Renderable);
            AddComponent(entity, new TakeUpGunTrigger { gunType = authoring._gunType });
        }
    }
}
public struct TakeUpGunTrigger : IComponentData
{
    public GunType gunType;
}
