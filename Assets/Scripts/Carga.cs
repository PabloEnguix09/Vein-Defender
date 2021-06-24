using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Carga : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string[] levelToLoad = ControladorCarga.nextLevel;
        StartCoroutine(this.Cargar(levelToLoad));

    }

    IEnumerator Cargar(string[] nivel)
    {

        AsyncOperation operation;
        yield return new WaitForSeconds(1f);
        foreach (string s in nivel)
        {
            operation = SceneManager.LoadSceneAsync(s,LoadSceneMode.Additive);
            while (operation.isDone == false)
            {
                yield return null;
            }
        }

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(nivel[0]));
        

        operation = SceneManager.UnloadSceneAsync("Game_Loading");
        while (operation.isDone == false)
        {
            yield return null;
        }


    }
}
