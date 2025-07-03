using UnityEngine;
using Chartacters;
using Zenject;

public class PlayerCharacterMonobeh : MonoBehaviour
{
   
    public Vector3 Position;
    public Quaternion Rotation;
    public static PlayerCharacterMonobeh instance;

    private void Start() {
        instance = this;
    }
    private void LateUpdate() {
        transform.position = Vector3.Lerp(transform.position, Position, Time.deltaTime*10);
        transform.rotation = Rotation;
    }
}
