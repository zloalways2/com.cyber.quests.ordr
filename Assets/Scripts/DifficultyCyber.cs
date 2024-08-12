using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultyCyber : MonoBehaviour
{


      public void BackCyber()
    {
        SceneManager.LoadScene(2);
    }

    public void EasyCyber()
    {
        PlayerPrefs.SetInt("LevelQuest", 0);
        PlayerPrefs.Save();
        SceneManager.LoadScene(6);
    }

    public void MiddleCyber()
    {
        PlayerPrefs.SetInt("LevelQuest", 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(6);
    }

    public void HardCyber()
    {
        PlayerPrefs.SetInt("LevelQuest", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene(6);
    }

    public void GoSettingsCyber()
    {
        PlayerPrefs.SetInt("LastQuest", 5);
        PlayerPrefs.Save();
        SceneManager.LoadScene(4);
    }
}
