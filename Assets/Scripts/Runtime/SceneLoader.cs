using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : Singleton<SceneLoader>
{
    public LoadingScreen loadingScreen;

    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
    }

    public void LoadSceneAsync(string name, bool showLoading = true)
    {
        //StopAllCoroutines();
        StartCoroutine("Progress", name);
    }

    private IEnumerator Progress(string name)
    {
        //Show loading screen
        loadingScreen.progress = 0f;//reset
        var loadingScreenWindow = loadingScreen.GetComponent<Window>();
        loadingScreenWindow.Show();
        while (loadingScreen.GetComponent<Window>().IsAnimating)
        {
            yield return null;
        }
        //Load scnen async
        var op = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
        op.allowSceneActivation = false;
        do
        {
            loadingScreen.progress = op.progress;
            if (op.progress >= 0.9f)
                op.allowSceneActivation = true;
            yield return null;
        } while (!op.isDone);
        loadingScreen.progress = 1f;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(name));
        yield return new WaitForSecondsRealtime(1f);
        loadingScreenWindow.Hide();


    }

    public void UnloadSceneAsync(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
}
