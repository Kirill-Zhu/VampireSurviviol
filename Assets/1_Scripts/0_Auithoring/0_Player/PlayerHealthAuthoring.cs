using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAuthoring : MonoBehaviour
{
    public int Health;

    class Baker : Baker<PlayerHealthAuthoring> {
        public override void Bake(PlayerHealthAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerHealth { Health = authoring.Health });
            AddBuffer<PlayerDamageBufferElement>(entity);
        }
    }

}
public struct PlayerHealth: IComponentData {
    public int Health;
}
public struct PlayerDamageBufferElement : IBufferElementData {
    public int Value;
}


