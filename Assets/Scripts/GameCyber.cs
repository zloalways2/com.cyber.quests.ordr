using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCyber : MonoBehaviour
{

    [SerializeField] GameObject[] frames;
    ItemCyber pressed;
    ItemCyber entered;
    [SerializeField] ItemCyber[] easy;
    [SerializeField] ItemCyber[] middle;
    [SerializeField] ItemCyber[] hard;
    [SerializeField] Sprite[] spritesCyber;
    List<int> ListQuest;
    bool orderStarted = false, EndOrder = false;
    private int CyberLevel;
    [SerializeField] Sprite ready;
    [SerializeField] Button ButtonOrder;
    [SerializeField] Text label;
    List<int> shuffled;

    [SerializeField] GameObject endframe;
    [SerializeField] Text endlabelCyber;
    [SerializeField] Sprite next;
    [SerializeField] Sprite again;
    [SerializeField] GameObject StarsCyber;
    [SerializeField] Text endscore;

    [SerializeField] AudioSource winS;
    [SerializeField] AudioSource loseS;
 
    private bool WinCyber = false;

    // Start is called before the first frame update
    void Start()
    {
        for (int orderit = 0; orderit < frames.Length; orderit++) frames[orderit].SetActive(false);
        CyberLevel = PlayerPrefs.GetInt("LastQuest", 0);
        frames[CyberLevel].SetActive(true);
        ListQuest = new List<int>();
        List<int> spritesInd = new List<int>();
        for (int orderit = 0; orderit < spritesCyber.Length; orderit++) spritesInd.Add(orderit);
        spritesInd.Shuffle();
        if(CyberLevel==0)
        {
            for (int cyberit = 0; cyberit < 3; cyberit++) ListQuest.Add(spritesInd[cyberit]);
            for(int cyberit = 0;cyberit<easy.Length;cyberit++)
            {
                easy[cyberit].image.sprite   = spritesCyber[ListQuest[cyberit]];
                easy[cyberit].IndCyber = ListQuest[cyberit];
            }
        } else if(CyberLevel==1)
        {
            for (int cyberit = 0; cyberit < 6; cyberit++) ListQuest.Add(spritesInd[cyberit]);
            for (int cyberit = 0; cyberit < middle.Length; cyberit++)
            {
                middle[cyberit].image.sprite = spritesCyber[ListQuest[cyberit]];
                middle[cyberit].IndCyber = ListQuest[cyberit];
            }
        } else
        {
            for (int orderit = 0; orderit < 9; orderit++) ListQuest.Add(spritesInd[orderit]);
            for (int orderit = 0; orderit < hard.Length; orderit++)
            {
                hard[orderit].image.sprite = spritesCyber[ListQuest[orderit]];
                hard[orderit].IndCyber = ListQuest[orderit];
            }
        }
    }

    public void BackCyber()
    {
        SceneManager.LoadScene(2);
    }

    public void SettingsCyber()
    {
        PlayerPrefs.SetInt("LastQuest", 6);
        PlayerPrefs.Save();
        SceneManager.LoadScene(4);
    }

    public void CyberGameUp()
    {
        if(orderStarted)
        {
            if (pressed != null && entered != null)
            {
                int tmp2 = pressed.IndCyber;
                pressed.IndCyber = entered.IndCyber;
                entered.IndCyber = tmp2;

                Sprite spriteCyber = entered.image.sprite;
                entered.image.sprite = pressed.image.sprite;
                pressed.image.sprite = spriteCyber;

               }
        }
    }

    public void Pressed(ItemCyber item)
    {
       if(orderStarted) pressed = item;
    }

    public void Enter(ItemCyber item)
    {
       if(orderStarted) entered = item;
    }

    public void StartCyber()
    {
        if(!orderStarted && !EndOrder)
        {
            orderStarted = true;
            label.text = "GUESS THE ORDER";
            shuffled = new List<int>();
            for (int orderit = 0; orderit < ListQuest.Count; orderit++) shuffled.Add(ListQuest[orderit]);
            shuffled.Shuffle();
            ButtonOrder.image.sprite = ready;
            if (CyberLevel == 0)
            {
                for (int orderit = 0; orderit < easy.Length; orderit++)
                {
                    easy[orderit].image.sprite = spritesCyber[shuffled[orderit]];
                    easy[orderit].IndCyber = shuffled[orderit];
                }
            }
            else if (CyberLevel == 1)
            {
                for (int orderit = 0; orderit < middle.Length; orderit++)
                {
                    middle[orderit].image.sprite = spritesCyber[shuffled[orderit]];
                    middle[orderit].IndCyber = shuffled[orderit];
                }
            }
            else
            {
                for (int orderit = 0; orderit < hard.Length; orderit++)
                {
                    hard[orderit].image.sprite = spritesCyber[shuffled[orderit]];
                    hard[orderit].IndCyber = shuffled[orderit];
                }
            }
        } else if(orderStarted && !EndOrder)
        {
            EndOrder = true;
            WinCyber = true;
            if(CyberLevel==0)
            {
                for (int orderit = 0; orderit < easy.Length; orderit++)
                {
                    if (ListQuest[orderit] != easy[orderit].IndCyber) WinCyber = false;
                }

            } else if(CyberLevel==1)
            {
                for (int orderit = 0; orderit < middle.Length; orderit++)
                {
                    if (ListQuest[orderit] != middle[orderit].IndCyber) WinCyber = false;
                }
            } else
            {
                for (int orderit = 0; orderit < hard.Length; orderit++)
                {
                    if (ListQuest[orderit] != hard[orderit].IndCyber) WinCyber = false;
                }
            }
            if (WinCyber) winS.PlayOneShot(winS.clip);
            else loseS.PlayOneShot(loseS.clip);
            endlabelCyber.text = WinCyber ? "GREAT!!!" : "GAME OVER";
            endframe.SetActive(true);
            ButtonOrder.image.sprite = WinCyber ? next : again;
            StarsCyber.SetActive(WinCyber);
        }  else if(EndOrder)
        {
            if(WinCyber)
            {
                CyberLevel++;
                CyberLevel = Mathf.Min(CyberLevel, 2);
                PlayerPrefs.SetInt("LastQuest", CyberLevel);
                PlayerPrefs.Save();
            }
            SceneManager.LoadScene(6);
        }
    }

}
