using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchManager : MonoBehaviour
{
    public enum SearchMode {
        body,
        face
    }

    [SerializeField] List<GameObject> _bodyPrefabList = new List<GameObject>();
    [SerializeField] List<GameObject> _facePrefabList = new List<GameObject>();

    private void Start()
    {
        Debug.Assert(_bodyPrefabList.Count == 0, "몸수색 관련 프리팹 넣어주세요");
        Debug.Assert(_facePrefabList.Count == 0, "얼굴수색 관련 프리팹 넣어주세요");
    }

    public void startSearch(SearchMode searchMode)
    {
        List<GameObject> bodySearchObjectList = new List<GameObject>();
        List<GameObject> faceSearchObjectList = new List<GameObject>();

        //여기서 데이터 결정

        switch (searchMode)
        {
            case SearchMode.body:
                {
                    FindObjectOfType<UISearch>().showSearchObject(ref bodySearchObjectList);
                }
                break;
            case SearchMode.face:
                {
                    FindObjectOfType<UISearch>().showSearchObject(ref faceSearchObjectList);
                }
                break;
            default:
                return;
        }
    }
}