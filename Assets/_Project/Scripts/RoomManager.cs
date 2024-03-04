using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

    [SerializeField]
    private Button startDebugButton;

    void Start()
    {
        startDebugButton.onClick.AddListener(()=>{
            StartDebug();
        });
    }

    void Update()
    {
        
    }

    void StartDebug()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("_Project/Scenes/Level_Forest", LoadSceneMode.Single);

        asyncOperation.allowSceneActivation = false;

        while (asyncOperation.progress < 0.9f)
        {
            Debug.Log(" progress = " + asyncOperation.progress);
        }

        asyncOperation.allowSceneActivation = true;

        yield return null;
        
        if(asyncOperation.isDone)
        {
            Debug.Log("完成加载");
        }
    }
}
