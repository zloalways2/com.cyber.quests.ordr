using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCyber : MonoBehaviour
{
    public void PlayCyber()
    {
        SceneManager.LoadScene(5);
    }
    public void OptionsCyber()
    {
        PlayerPrefs.SetInt("LastQuest", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene(4);
    }
    public void PolicyCyber1()
    {
        PolicyCyber.IND_BACK = 2;
        SceneManager.LoadScene(3);
    }
    public void QuitCyber()
    {
        Application.Quit();
    }

}
