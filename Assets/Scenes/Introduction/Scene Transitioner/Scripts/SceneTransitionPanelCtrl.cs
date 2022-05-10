using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//using UnityEditor.Animations;
using UnityEngine.SceneManagement;

/* Summnary of "How to Use"
 * 
 * 1) Add this script to the object that will invoke the transition.  Do not
 * modify it.
 * 2) Configure its options in the inspector
 * 3) Invoke it from the second script with the command
 * 
 *      GetComponent<SceneTransitionPanelCtrl>().Transition();
 */


/* SceneTransitionPanelCtrl script
 * 
 * This script is part of the Scene Transitioner package.  The package includes
 * the top-level "Scene Transitioner" object which houses a canvas (TransitionCanvas)
 * and an EventSystem.  The transition canvas, in turn, contains a panel
 * (TransitionPanel).  The panel houses the animator which implements the
 * transitions.
 * 
 * Attach this script to the object in your project that will invoke the transition.
 * The script performs 3 thing:
 * 1) It executes the "fade out" animation on the current scene.
 * 2) It waits for the "fade out" to complete.
 * 3) It loads the next scene.
 * 
 * There are 4 ways of specifying the next scene.  You can
 * 1) specify the scene's name
 * 2) specify the scene's build number
 * 3) specify the next scence (current scene's build number + 1)
 * 4) specify the previous scence (current scene's build number - 1)
 * 
 * Use the public enum sceneToLoad to specify how the next scene should be
 * chosen.  Depending of which option is used will depend on whether or not
 * the public members sceneName and sceneBuildNumber are used.
 * 
 * To execute the scene transition, execute the following line of code
 * from your script:
 * 
 *      GetComponent<SceneTransitionPanelCtrl>().Transition();
 *             
 * Note that your object will have at least two scripts:  this script (which
 * you should not modify) and a script that will invoke the transition (calls
 * the Transition() function in this script using the above command.
 * 
 * Prior to invoking the script, set the panel parameters ("Scene To Load", 
 * "Scene Name", "Scene Build Number", etc.)
 * 
 * The script uses a coroutine that permits 1.25 seconds for the fade out
 * transition to occur before it invokes the scene transition.  Not that the
 * 1.25 seconds value is currently a hard-coded number.  If you wish to
 * change the fade out animation, you may need to change this value.
 * 
 * The transition is performed with a pair of animations.  Ont that fades in
 * and another that fades out.  You can replace these animations to change
 * the transition effect.
 * 
 * JVolcy 1/1/21
 */
public class SceneTransitionPanelCtrl : MonoBehaviour
{
    //public Animator transitionAnimator;
    public enum NextScene { UseSceneName, UseSceneBuildNumber, UseNextBuildNumber, UsePrevBuildNumber };
    public NextScene sceneToLoad;
    public string sceneName;
    public int sceneBuildNumber;

    /* call the Transition() function to execute the transition. */
    public void Transition()
    {
        StartCoroutine(LoadScene());
    }


    //LoadScene() is a coroutine for sequencing the fade out in the current
    //scene before executing the scene change.  The corourine allows us to
    //delay the scene change by enough time to allow the fade out animation
    //to execute.
    IEnumerator LoadScene()
    {
        GameObject transPanel = GameObject.FindWithTag("TransitionPanel");
        //Debug.Log("transPanel= " + transPanel.tag);
        Animator panelAnimator = transPanel.GetComponentInChildren<Animator>();
        //Debug.Log("panelAnimator= " + panelAnimator.name);

        //invoke the animation trigger in the Animator
        panelAnimator.SetTrigger("fadeOut");

        yield return new WaitForSeconds(2f);     //give enough time for the 1 second animation to complete

        //now, transition to the new scene depending on which method the user
        //used to specify the next scene.
        switch (sceneToLoad)
        {
            case NextScene.UseSceneName:
                {
                    SceneManager.LoadScene(sceneName);
                    break;
                }
            case NextScene.UseSceneBuildNumber:
                {
                    SceneManager.LoadScene(sceneBuildNumber);
                    break;
                }
            case NextScene.UseNextBuildNumber:
                {
                    //use the current scene build number +1 to load the next scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                    break;
                }
            case NextScene.UsePrevBuildNumber:
                {
                    //use the ucrrent scen build number -1 to load the next scene
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                    break;
                }
        }
    }
    
}
