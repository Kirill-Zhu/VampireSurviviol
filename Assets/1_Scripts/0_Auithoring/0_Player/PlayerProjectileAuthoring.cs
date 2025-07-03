using Unity.Entities;
using UnityEngine;

public class PlayerProjectileAuthoting: MonoBehaviour {

    public float Damage;
    class Baker : Baker<PlayerProjectileAuthoting> {
        public override void Bake(PlayerProjectileAuthoting authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerProjectile { Damage = authoring.Damage });
        }
    }
}
public struct PlayerProjectile: IComponentData
{
    public float Damage;
}
