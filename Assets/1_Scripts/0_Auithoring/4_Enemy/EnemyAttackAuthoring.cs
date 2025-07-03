using Unity.Entities;
using UnityEngine;

public class EnemyAttackAuthoring : MonoBehaviour
{
    public int Damage;
    public float AttackTime;
    public float AttackRate;
    public float AttackRange;

    class Beker : Baker<EnemyAttackAuthoring> {
        public override void Bake(EnemyAttackAuthoring authoring) {
          Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new EnemyAttack { 
                Damage = authoring.Damage,
                AttackTime = authoring.AttackTime,
                AttackRate = authoring.AttackRate,  
                AttackRange = authoring.AttackRange
            });
        }
    }
}
public struct EnemyAttack : IComponentData {
    public int Damage;
    public float AttackTime;
    public float AttackRate;
    public float AttackRange;
}