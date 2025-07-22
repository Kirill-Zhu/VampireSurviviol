using Unity.Entities;
using UnityEngine;

public class ControlledGunAuthoring: MonoBehaviour {
    public GameObject BulletPrefab;
    public TypeOfControlledBullet TypeOfBullet;
    public float ReloadTimer;
    public float RateOfFire;
    public class Baker : Baker<ControlledGunAuthoring> {
        public override void Bake(ControlledGunAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new ControlledGun {
                BulletPrefab = GetEntity(authoring.BulletPrefab, TransformUsageFlags.Dynamic),
                TypeOfBulelt = authoring.TypeOfBullet, 
                ReloadTimer = authoring.ReloadTimer,
                RateOfFire = authoring.RateOfFire,
            });
        }
    }

}
public struct ControlledGun : IComponentData
   {public Entity BulletPrefab;
    public TypeOfControlledBullet TypeOfBulelt;
    public float ReloadTimer;
    public float RateOfFire;
    public bool IsSootingButtonDown;
}
public enum TypeOfControlledBullet {
    None = 0,   
    Pistol = 1,
}