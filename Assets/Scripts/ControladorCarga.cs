using UnityEngine;
using UnityEngine.SceneManagement;

public class ControladorCarga : MonoBehaviour
{
    public static string[] nextLevel;
    public static void LoadLevel(string[] name)
    {
        nextLevel = name;
        SceneManager.LoadScene("Game_Loading");
    }
}
