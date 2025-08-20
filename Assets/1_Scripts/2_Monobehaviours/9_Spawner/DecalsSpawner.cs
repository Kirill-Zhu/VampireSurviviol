using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DecalsSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _bloodDecalList;
    private int _prevIndex=0;
    public static DecalsSpawner Instance;
    private void Awake() {

        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }
    public void SpawnDecal(DecalType decalType, Vector3 pos) {
        //Need do switch by DecalType

        for (int i = 0; i < _bloodDecalList.Count; i++) {
            if (i != _prevIndex)
                continue;

            if (!_bloodDecalList[i].gameObject.activeInHierarchy)
                _bloodDecalList[i].gameObject.SetActive(true);
           
                _bloodDecalList[i].transform.position = pos;
            if (_prevIndex < _bloodDecalList.Count-1) {
                _prevIndex++;
            } else 
                _prevIndex = 0;
            break;
        }

      
    }
}

public enum DecalType {
    Blood = 0,

}