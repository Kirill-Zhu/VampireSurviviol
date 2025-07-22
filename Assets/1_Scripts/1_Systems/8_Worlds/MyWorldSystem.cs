using Unity.Burst;
using Unity.Entities;
using UnityEngine;

partial class MyWorldSystem : MonoBehaviour
{
    private void Start() {
        World world = new World("TestWorld", WorldFlags.None);
    }
}
