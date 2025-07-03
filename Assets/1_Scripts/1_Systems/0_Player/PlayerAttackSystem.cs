using Unity.Burst;
using Unity.Entities;
using Unity.Physics;

public partial struct PlayerAttackSystem: ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) { 
         
    }
}
struct CollissionJob : ICollisionEventsJob {
    public void Execute(CollisionEvent collisionEvent) {
        
    }
}
