using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public void ContinueClick()
    {
        SceneManager.LoadScene("Assignment Base");
    }

    public void RestartClick()
    {
        SceneManager.LoadScene("Assignment Base");
    }

    public void QuitClick()
    {
        Application.Quit();
    }
}
