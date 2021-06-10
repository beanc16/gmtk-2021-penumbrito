using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BeforeSceneLoadedEvent : UnityEvent { }

[System.Serializable]
public class AfterSceneLoadedEvent : UnityEvent { }

[System.Serializable]
public class SceneLoadFailedEvent : UnityEvent { }

[System.Serializable]
public class BeforeExitGameEvent : UnityEvent { }



public class SceneHandler : MonoBehaviour
{
    private static SceneHandler instance = new SceneHandler();

    // Load events (works for reloads / restarts too)
    public static BeforeSceneLoadedEvent OnBeforeSceneLoaded;
    
    public static AfterSceneLoadedEvent OnAfterSceneLoaded;
    
    public static SceneLoadFailedEvent OnSceneLoadFailed;

    // Exit events
    public static BeforeExitGameEvent OnBeforeExitGame;



    public static void LoadScene(string sceneName)
    {
        // The scene CAN be loaded
        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            TryCallOnBeforeSceneLoadedEvent();  // OnBeforeSceneLoaded()
            SceneManager.LoadScene(sceneName);  // Load scene
            TryCallOnAfterSceneLoadedEvent();   // OnAfterSceneLoaded()
        }

        // The scene CAN NOT be loaded
        else
        {
            Debug.LogError("ERROR - Scene: " + sceneName + " could not be loaded.");
            TryCallOnSceneLoadFailedEvent();    // OnSceneLoadFailed()
        }
    }
	
	public static void LoadSceneAfterSeconds(string sceneName, float seconds)
	{
		IEnumerator coroutine = LoadSceneAfterSecondsPassed(sceneName, seconds);
        instance.StartCoroutine(coroutine);
	}
    
    public static void ReloadCurrentScene()
    {
        Scene curScene = SceneManager.GetActiveScene();
        LoadScene(curScene.name);
    }
    
    public static void ReloadCurrentSceneAfterSeconds(float seconds)
    {
        Scene curScene = SceneManager.GetActiveScene();
        LoadSceneAfterSeconds(curScene.name, seconds);
    }

    public static void RestartCurrentScene()
    {
        ReloadCurrentScene();
    }
    
    public static void RestartCurrentSceneAfterSeconds(float seconds)
    {
        ReloadCurrentSceneAfterSeconds(seconds);
    }
    
    public static void ExitGame()
    {
        TryCallOnBeforeExitGameEvent();         // OnBeforeExitGame()
        Application.Quit();
    }
	
	
	
	private static IEnumerator LoadSceneAfterSecondsPassed(string sceneName, float seconds)
	{
		yield return new WaitForSeconds(seconds);
		LoadScene(sceneName);
	}



    private static void TryCallOnAfterSceneLoadedEvent()
    {
        if (OnAfterSceneLoaded != null)
        {
            OnAfterSceneLoaded.Invoke();
        }
    }

    private static void TryCallOnBeforeSceneLoadedEvent()
    {
        if (OnBeforeSceneLoaded != null)
        {
            OnBeforeSceneLoaded.Invoke();
        }
    }

    private static void TryCallOnSceneLoadFailedEvent()
    {
        if (OnSceneLoadFailed != null)
        {
            OnSceneLoadFailed.Invoke();
        }
    }

    private static void TryCallOnBeforeExitGameEvent()
    {
        if (OnBeforeExitGame != null)
        {
            OnBeforeExitGame.Invoke();
        }
    }
}
