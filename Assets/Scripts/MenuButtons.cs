// ---------------------------------------------------
// NAME: MenuButtons
// STATUS: DONE
// GAMEOBJECT: EventManager
// DESCRIPTION: Manager for menu buttons, start and exit game
//
// AUTHOR: Luis Belloch
// FEATURES ADDED: onClick events
// ---------------------------------------------------


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{

    public int GameScene;

    public void OnClickStart()
    {
        SceneManager.LoadScene(GameScene);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
