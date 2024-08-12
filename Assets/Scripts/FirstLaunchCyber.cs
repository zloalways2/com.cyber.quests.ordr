using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstLaunchCyber : MonoBehaviour
{

    public void AcceptPolicyCyber()
    {
        PlayerPrefs.SetInt("OpenScene", 2);
        PlayerPrefs.Save();
        SceneManager.LoadScene(2);
    }

    public void OpenPolicyCyber()
    {
        PolicyCyber.IND_BACK = 1;
        SceneManager.LoadScene(3);
    }
}
