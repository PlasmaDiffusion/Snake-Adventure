using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MonoBehaviour
{
    [Header("UI referemces")]

    public Text title1;
    public Text desc1;
    public Text title2;
    public Text desc2;

    public Image image1;
    public Image image2;

    public Button prevButton;
    public Button nextButton;
    public Button quitButton;

    [Header("Text and images here!")]

    public string[] titles;
    public string[] descs;
    public Sprite[] images;
    public Vector2[] imageSizes;


    int currentIndex;

    int maxIndex;

    // Start is called before the first frame update
    void Start()
    {
        currentIndex = 0;


        //Max index should be array 
        maxIndex = titles.Length - 2;
        
        //Prepare buttons
        nextButton.onClick.AddListener(Next);
        prevButton.onClick.AddListener(Prev);
        quitButton.onClick.AddListener(Exit);

        UpdateHelpPage();
    }

    void UpdateHelpPage()
    {
        title1.text = titles[currentIndex];
        title2.text = titles[currentIndex + 1];

        desc1.text = descs[currentIndex];
        desc2.text = descs[currentIndex + 1];

        image1.sprite = images[currentIndex];
        ResizeImage(image1, currentIndex);
        image2.sprite = images[currentIndex + 1];
        ResizeImage(image2, currentIndex+1);

    }

    //Set any hard code image sizes here
    void ResizeImage(Image image, int index)
    {
        //Default size
        if (imageSizes[index].x == 0.0f)
        {
            image.transform.localScale = new Vector2(1.0f, 1.0f);
        }
        else
        {
            image.transform.localScale = imageSizes[index];
        }
    }

    void Next()
    {
        if (currentIndex < maxIndex) currentIndex += 2;
        else currentIndex = 0;
        UpdateHelpPage();
    }

    void Prev()
    {
        if (currentIndex > 0) currentIndex -= 2;
        else currentIndex = maxIndex;
        UpdateHelpPage();
    }

    void Exit()
    {
        GlobalStats.hud.ChangeMenuMode(1);
    }
}
