using UnityEngine;
using UnityEngine.SceneManagement;

public class PolicyCyber : MonoBehaviour
{


    public void GoBackCyber()
    {
        SceneManager.LoadScene(IND_BACK);
    }


    public static int IND_BACK = 2;
}
