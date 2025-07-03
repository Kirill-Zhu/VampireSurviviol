using Unity.Entities;
using UnityEngine;

public class GunAuthoring: MonoBehaviour {
    public GameObject Bullet;
    public float ReloadTimer;

    public class GunAuthoringBaker : Baker<GunAuthoring> {
        public override void Bake(GunAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new GunA {

                Bullet = GetEntity(authoring.Bullet, TransformUsageFlags.Dynamic),
                ReloadTimer = authoring.ReloadTimer
            });
        }
    }

}
public struct GunA : IComponentData
{
    public Entity Bullet;
    public float ReloadTimer;

}
