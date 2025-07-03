using UnityEngine;

public class SkillsMonobehAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _ulitParitcle;
    private void Update() {
        if (PlayerInputs.Instance.UltiAction.WasPressedThisFrame()) {
            foreach (var particle in _ulitParitcle) { 
            particle.Play();
            }
        }
    }
}
