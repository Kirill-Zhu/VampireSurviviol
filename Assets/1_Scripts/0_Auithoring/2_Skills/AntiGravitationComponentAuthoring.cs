using Unity.Entities;
using UnityEngine;

public class AntiGravitationComponentAuthoring : MonoBehaviour
{
    [SerializeField] private float _timeToFly = 2;
    class Baker : Baker<AntiGravitationComponentAuthoring> {
        public override void Bake(AntiGravitationComponentAuthoring authoring) {
          Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new AntiGravitationComponent { TimeToFly = authoring._timeToFly, Timer = 0});
        }
    }
}
public struct AntiGravitationComponent: IComponentData {
    public float TimeToFly;
    public float Timer;
}