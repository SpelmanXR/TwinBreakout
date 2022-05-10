using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > 5f || Input.anyKeyDown)
        {
            SceneTransitionPanelCtrl sceneTransitionPanelCtrl = GetComponent<SceneTransitionPanelCtrl>();
            sceneTransitionPanelCtrl.Transition();
            //SceneManager.LoadScene(1);
        }
    }
}
