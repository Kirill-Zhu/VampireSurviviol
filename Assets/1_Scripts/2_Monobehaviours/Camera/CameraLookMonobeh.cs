using Unity.Cinemachine;
using UnityEngine;

public class CameraLookMonobeh : MonoBehaviour
{
    [SerializeField] private CinemachineRotationComposer _composer;
    [SerializeField] private float _composerMultiplier=0.2f;
    [SerializeField] private float _lerpMultiplier = 6f;
    private Vector2 _cameraLookVector;

    private void Update() {
        
        _cameraLookVector = PlayerInputs.Instance.CameraLookVector;
        if (_cameraLookVector != Vector2.zero) {
            _composer.Composition.ScreenPosition.x = -_cameraLookVector.x*_composerMultiplier;
            _composer.Composition.ScreenPosition.y = _cameraLookVector.y*_composerMultiplier;
        } else if (_cameraLookVector == Vector2.zero) { 
            _composer.Composition.ScreenPosition = Vector2.Lerp(_composer.Composition.ScreenPosition, Vector2.zero, Time.deltaTime * _lerpMultiplier);
        }
    }
}
