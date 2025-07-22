using Unity.Entities;
using UnityEngine;

public class EnemyHealthAuthoring : MonoBehaviour
{
    public int Health;
    class Baker : Baker<EnemyHealthAuthoring> {
        public override void Bake(EnemyHealthAuthoring authoring) {
          Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemyHealth { Health = authoring.Health});
        }
    }
}
public struct EnemyHealth : IComponentData {
    public int Health;
    public void DoDamage(int DamageValue) {
        Health -= DamageValue;
        if (Health <= 0) {

        }
    }
}
public  struct EnemyDamageBufferElement: IBufferElementData {
    public int Value;
}