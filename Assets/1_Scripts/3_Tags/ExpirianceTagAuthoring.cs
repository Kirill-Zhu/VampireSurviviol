using Unity.Entities;
using UnityEngine;

public class ExpirianceTagAuthoring: MonoBehaviour {

    class Baker : Baker<ExpirianceTagAuthoring> {
        public override void Bake(ExpirianceTagAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ExpirianceTag{ });
        }
    }
}

public struct ExpirianceTag:IComponentData {

}