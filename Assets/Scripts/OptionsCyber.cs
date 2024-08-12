using UnityEngine;
using UnityEngine.UI;
using CyberQuest.SoundManagerOrder;
using UnityEngine.SceneManagement;

public class OptionsCyber : MonoBehaviour
{

    [SerializeField] Slider SoundsSliderQuest;
    [SerializeField] Slider SliderMusicOrder;

    void Start()
    {
        SoundsSliderQuest.value = SoundManagerCyber.SoundVolume;
        SliderMusicOrder.value = SoundManagerCyber.MusicVolume;
    }

     public void BackCyber()
        {
        SceneManager.LoadScene(PlayerPrefs.GetInt("LastQuest", 2));
        }

    public void ChangeSoundsCyber()
    {
        PlayerPrefs.SetFloat("SoundQuest", SoundsSliderQuest.value);
        PlayerPrefs.Save();
        SoundManagerCyber.SoundVolume = SoundsSliderQuest.value;
    }

    public void ChangeMusicCyber()
    {
        PlayerPrefs.SetFloat("MusicQuest", SliderMusicOrder.value);
        PlayerPrefs.Save();
        SoundManagerCyber.MusicVolume = SliderMusicOrder.value;
    }
}
