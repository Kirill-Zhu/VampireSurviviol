using UnityEngine;

namespace Chartacters {
    [CreateAssetMenu(fileName = "New Character", menuName = "Custom/New Character")]
    public class Character : ScriptableObject {

        public Mesh Mesh;
        public Material Material;
        public float Speed = 10;
        public float RotationSpeed = 10;    

    }
}