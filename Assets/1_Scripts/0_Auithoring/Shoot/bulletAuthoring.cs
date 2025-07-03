using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour
{
    public float Speed;
    public float LiveTime;
    public float Timer;
    class BulletBaker : Baker<BulletAuthoring> {
        public override void Bake(BulletAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet { Speed = authoring.Speed, LiveTime = authoring.LiveTime, Timer = authoring.Timer });
        }
    }
}

public struct Bullet: IComponentData {
    public float Speed;
    public float LiveTime;
    public float Timer;
}