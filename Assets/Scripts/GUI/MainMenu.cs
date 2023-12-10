using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("WorldMap");
    }

    public void MainScreen()
    {
        SceneManager.LoadScene("MainScreen");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public AudioMixer audioMixer;
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }
}
