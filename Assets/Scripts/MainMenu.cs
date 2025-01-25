using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject music;
    private static bool musicExists;

    void Start() {
        if (musicExists) {
            Destroy(music);
        } else {
            musicExists = true;
            DontDestroyOnLoad(music);
        }
    }

    public void LoadGame() {
        SceneManager.LoadScene("MainGame");
    }
}
