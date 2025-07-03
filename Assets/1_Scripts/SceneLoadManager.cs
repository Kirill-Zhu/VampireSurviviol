using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoadManager : MonoBehaviour
{
    [ContextMenu("LoadScene 1")]
  public void LoadScene() {
        SceneManager.LoadScene(1);
    }
}
