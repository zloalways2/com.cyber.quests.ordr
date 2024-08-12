using UnityEngine;
using UnityEngine.UI;

public class ItemCyber : MonoBehaviour
{
    [SerializeField] GameCyber GameCyber;
    public int IndCyber;
    [SerializeField] public Image image;

    public void EnterCyber()
    {
        GameCyber.Enter(this);
    }
    public void PressedCyber()
    {
        GameCyber.Pressed(this);
    }

    public void UpCyber()
    {
        GameCyber.CyberGameUp();
    }
}
