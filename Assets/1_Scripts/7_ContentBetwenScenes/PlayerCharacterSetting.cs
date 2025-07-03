using UnityEngine;

[CreateAssetMenu(  fileName ="PlayerCharacterSetting", menuName = "Custom/PlayerCharacterSettings")]
public class PlayerCharacterSetting : ScriptableObject
{
    [Header("Entities")]
    public float Speed => _speed;
    public float RotationSpeed => _rotationSpeed;   
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;
    public GameObject Graphics => _graphics;
    [Space (2)]
    [Header("Monobehaviour")]
    [SerializeField] private GameObject _graphics;
}
