using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShaderHandler : MonoBehaviour
{
    [SerializeField] private Material[] _materials;

    [SerializeField] private float _value;

    private void Start() {
         _materials = GetComponent<MeshRenderer>().materials;
    }
    private void Update() {
        foreach (var mat in _materials) { 
            mat.SetFloat("_RandomSeed", _value);
        }
    }
}
