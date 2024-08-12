using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]

public class ProgressBarQuest : MonoBehaviour
{

    
    [Header("Bar Setting1")]
    public Color BarColor1;   
    public Color BarBackGroundColor1;
    public Sprite BarBackGroundSprite1;
    
    public bool repeat1 = false;
    public float RepeatRate1 = 1f;

    private Image bar, barBackground;
    private float barValues1;
    public float BarValue
    {
        get { return barValues1; }

        set
        {
            value = Mathf.Clamp(value, 0, 100);
            barValues1 = value;
            UpdateValue(barValues1);

        }
    }

        

    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        barBackground = GetComponent<Image>();
        barBackground = transform.Find("BarBackground").GetComponent<Image>();
    }

    private void Start()
    {
        bar.color = BarColor1;
        barBackground.color = BarBackGroundColor1; 
        barBackground.sprite = BarBackGroundSprite1;

        UpdateValue(barValues1);


    }

    void UpdateValue(float val)
    {
        bar.fillAmount = val / 100;

        bar.color = BarColor1;

    }


    private void Update()
    {
        if (!Application.isPlaying)
        {           
            UpdateValue(50);
           
            bar.color = BarColor1;
            barBackground.color = BarBackGroundColor1;

            barBackground.sprite = BarBackGroundSprite1;           
        }
        else
        {
        }
    }

}
