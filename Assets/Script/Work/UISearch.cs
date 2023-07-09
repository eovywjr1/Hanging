using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISearch : MonoBehaviour
{
    //조민수 : startSearch 함수를 수색 시작하는 버튼에 넣으면 좋을듯
    public void startFaceSearch()
    {
        SearchManager searchManager = FindObjectOfType<SearchManager>();
        if (searchManager == null)
            return;

        searchManager.startSearch(SearchManager.SearchMode.face);
    }

    public void startBodySearch()
    {
        SearchManager searchManager = FindObjectOfType<SearchManager>();
        if (searchManager == null)
            return;

        searchManager.startSearch(SearchManager.SearchMode.body);
    }

    public void showSearchObject(ref List<GameObject> objectList)
    {

    }    

    //조민수 : report 함수를 신고 버튼에 넣으면 좋을듯
    public void report()
    {
        FindObjectOfType<HangingManager>().searchReport();
    }
}
