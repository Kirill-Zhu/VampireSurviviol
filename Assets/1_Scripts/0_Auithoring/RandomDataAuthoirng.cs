using Unity.Entities;

using UnityEngine;
using Random = Unity.Mathematics.Random;
public class RandomDataAuthoirng : MonoBehaviour
{
    public Random Value;
    class Baker : Baker<RandomDataAuthoirng> {
        public override void Bake(RandomDataAuthoirng authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new RandomData { Value = authoring.Value });
        }
    }
}
public struct RandomData : IComponentData {
    public Random Value;
}