using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class UnitMoverAuthoring : MonoBehaviour
{
    public float MoveSpeed = 3;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public float RotationSpeed = 3;
    public class Baker: Baker<UnitMoverAuthoring> {
        public override void Bake (UnitMoverAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitMover { 
              MoveSpeed = authoring.MoveSpeed,
              MinMoveSpeed = authoring.MinMoveSpeed,
              MaxMoveSpeed = authoring.MaxMoveSpeed,
              RotationSpeed = authoring.RotationSpeed });

        }
    }

}

public struct UnitMover:IComponentData {
   
    public float RotationSpeed;
    public float3 InputTargetPosition;

    public float MoveSpeed;
    public float MinMoveSpeed;
    public float MaxMoveSpeed;
    public static UnitMover SetSpeed(float moveSpeed, float rotaionSpeed) => new UnitMover { MoveSpeed = moveSpeed, RotationSpeed = rotaionSpeed };
   
}