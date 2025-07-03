using System.Collections.Generic;
using UnityEngine;

public class BakedAnimationHandler : MonoBehaviour
{
    [SerializeField] private List<Material> _materials;


    private void Start() {
        foreach(Material mat in _materials) {
            float random = Random.Range(0, 2);
            mat.SetFloat("RandomSeed", random);
            Debug.Log("Random Seed material is: " + random); 
        }
    }
}
