using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject escMenuHolder;
    public GameObject overlayHolder;

    void Start()
    {
        DisableMenu();
    }

    public void EnableMenu()
    {
        escMenuHolder.SetActive(true);
        overlayHolder.SetActive(false);
    }

    public void DisableMenu()
    {
        escMenuHolder.SetActive(false);
        overlayHolder.SetActive(true);
    }

    public void Resume()
    {
        DisableMenu();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Link()
    {
        Application.OpenURL("http://www.google.com");
    }
}
