using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void NW()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void FR()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void MN()
    {
        SceneManager.LoadSceneAsync(3);
    }
    public void WH()
    {
        SceneManager.LoadSceneAsync(4);
    }
}
