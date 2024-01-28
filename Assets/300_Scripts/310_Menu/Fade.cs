using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    public CanvasGroup FadeImage;
    public float fadeSpeed;
    public void In(int sceneIndex)
    {
        StartCoroutine(InCour(sceneIndex));
    }

    public void Out()
    {
        StartCoroutine(OutCour());
    }

    IEnumerator InCour(int sceneIndex)
    {
        while (FadeImage.alpha < 1)
        {
            FadeImage.alpha += fadeSpeed;

            Debug.Log("Iterate In");
            yield return new WaitForSeconds(.01f);
        }
        SceneManager.LoadScene(sceneIndex);
    }

    IEnumerator OutCour()
    {
        while (FadeImage.alpha > 0)
        {
            FadeImage.alpha -= fadeSpeed;

            Debug.Log("Iterate Out");
            yield return new WaitForSeconds(.01f);
        }
    }
}
