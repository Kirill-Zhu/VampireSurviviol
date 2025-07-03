using Unity.Entities;
using Unity.Rendering;
using UnityEngine;
public class RandomSeedShaderPropertyAuthoring: MonoBehaviour {
    public float Value;
    class Baker : Baker<RandomSeedShaderPropertyAuthoring> {
        public override void Bake(RandomSeedShaderPropertyAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RandomSeedPropertyShader { Value = authoring.Value});
        }
    }
}
[MaterialProperty("_RandomSeed")]
public struct RandomSeedPropertyShader : IComponentData
{
    public float Value;
}
