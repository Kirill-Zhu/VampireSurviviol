using System.ComponentModel.Design;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMoverAuthoring : MonoBehaviour {
    public float Speed;
    public float RotationSpeed;
    class Baker : Baker<PlayerMoverAuthoring> {
        public override void Bake(PlayerMoverAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerMover { MoveSpeed = authoring.Speed, RotationSpeed = authoring.RotationSpeed});
        }
    }
}
public struct PlayerMover: IComponentData {
    public float MoveSpeed;
    public float RotationSpeed;
    public float3 InputTargetPosition;
    public float3 Rotation;
}