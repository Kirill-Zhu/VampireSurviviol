using Unity.Entities;
using UnityEngine;

public class EnemyJumperAuthoring : MonoBehaviour
{
    public float JumpForce = 10;
    public float JumpUpForce = 20;
    public float JumpRate = 4;
    class Baker : Baker<EnemyJumperAuthoring> {
        public override void Bake(EnemyJumperAuthoring authoring) {
             Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EnemyJumper { 
                JumpForce = authoring.JumpForce, 
                JumpUpForce = authoring.JumpUpForce, 
                JumpRate = authoring.JumpRate, 
                JumpReloadTimer = authoring.JumpRate});  
        }
    }

}
public struct EnemyJumper: IComponentData {

    public float JumpForce;
    public float JumpUpForce;
    public float JumpRate;
    public float JumpReloadTimer;
}