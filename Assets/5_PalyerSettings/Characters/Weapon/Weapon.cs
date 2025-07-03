using UnityEngine;

namespace Chartacters {
    [CreateAssetMenu(fileName = "New Weapon", menuName = "Custom/New Weapon")]
    public class Weapon : ScriptableObject 
    {
        public Mesh BulletMesh;
        public Material BulletMaterial;
        public float Damage = 1;
    }
}