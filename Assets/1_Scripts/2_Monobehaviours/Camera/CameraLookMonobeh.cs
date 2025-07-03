using Unity.Cinemachine;
using UnityEngine;

public class CameraLookMonobeh : MonoBehaviour
{
    [SerializeField] private CinemachineRotationComposer _composer;

    private Vector2 _cameraLookVector;

    private void Update() {
        
        _cameraLookVector = PlayerInputs.Instance.CameraLookVector;
        if (_cameraLookVector != Vector2.zero) {
            _composer.Composition.ScreenPosition.x = -_cameraLookVector.x;
            _composer.Composition.ScreenPosition.y = _cameraLookVector.y*0.8f;
        } else if (_cameraLookVector == Vector2.zero) { 
            _composer.Composition.ScreenPosition = Vector2.Lerp(_composer.Composition.ScreenPosition, Vector2.zero, Time.deltaTime * 6);
        }
    }
}
