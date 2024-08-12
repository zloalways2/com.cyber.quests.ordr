using CyberQuest.SoundManagerOrder;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoaderCyber : MonoBehaviour
{
    [SerializeField] AudioSource MusicCyber;
    [SerializeField] ProgressBarQuest progressBarCyber;
    float CyberTimer = 0;

    void Start()
    {   
        SoundManagerCyber.MusicVolume = PlayerPrefs.GetFloat("MusicQuest", 1);
        SoundManagerCyber.SoundVolume = PlayerPrefs.GetFloat("SoundQuest", 1);
        progressBarCyber.BarValue = 0;
        MusicCyber.PlayLoopingMusicManagedCyber(1.0f, 1.0f, true);
    }

    void Update()
    {
        if(Time.timeSinceLevelLoad>=CyberTimer)
        {
            CyberTimer = CyberTimer +  0.03f;
            progressBarCyber.BarValue+=1;
        }
        if(progressBarCyber.BarValue==100)
        {
            int StartSceneQuest = PlayerPrefs.GetInt("OpenScene", 1);
            SceneManager.LoadScene(StartSceneQuest);
        }
    }
}
