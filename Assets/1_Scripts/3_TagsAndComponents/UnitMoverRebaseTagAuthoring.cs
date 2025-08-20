using Unity.Entities;
using UnityEngine;

public class UnitMoverRebaseTagAuthoring: MonoBehaviour
{
    class Baker : Baker<UnitMoverRebaseTagAuthoring> {
        public override void Bake(UnitMoverRebaseTagAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new UnitMoverRebaseTag());
        }
    }
}
public struct UnitMoverRebaseTag: IComponentData {

}