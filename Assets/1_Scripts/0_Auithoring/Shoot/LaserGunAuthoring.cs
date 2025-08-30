using Unity.Entities;
using UnityEngine;

public class LaserGunAuthoring : MonoBehaviour {
    [SerializeField] private bool _isShooting = false;
    [SerializeField] private int _damage = 1;
    [SerializeField] private float _damageTime = 1;
    class Baker : Baker<LaserGunAuthoring> {
        public override void Bake(LaserGunAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new LaserGun { isShooting = authoring._isShooting, 
                damage = authoring._damage, 
                timer = 0, 
                damageTime = authoring._damageTime});
         
        }
    }
}

public struct LaserGun : IComponentData, IEnableableComponent {
    public bool isShooting;   
    public int damage;
    public float timer;// How long player is shooting with this laser(after 1 sec need doo damage and reset or increase damage)
    public float damageTime;
}
