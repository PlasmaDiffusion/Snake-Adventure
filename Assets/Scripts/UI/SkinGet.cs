using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Lerp open the present!
public class SkinGet : MonoBehaviour
{
    public Text newSkinName;
    public Image newSkinIcon; //This gets set as the object is created with SetSkinIcon();
    public Image presentTop;
    public Image presentBottom;
    public Button tryItButton;

    [Header ("Set all icons here.")]
    public Image[] snakeIcons;
    public Image[] themeIcons;

    //Lerp values for opening the present
    float topPresentT;
    float bottomPresentT;
    Vector2 topStart;
    Vector2 bottomStart;

    //Lerp value for the icon growing
    float skinIconT;

    //Which skin is it
    bool isLevelTheme;

    // Start is called before the first frame update
    void Start()
    {
        //Record the present icon starting positions
        topStart = presentTop.rectTransform.position;
        bottomStart = presentBottom.rectTransform.position;

        //Init lerp time values
        topPresentT = 0.0f;
        bottomPresentT = 0.0f;
        skinIconT = 0.0f;

        //Make the stuff that'll grow start small
        tryItButton.transform.localScale = new Vector2(0.0f, 0.0f);
        newSkinName.transform.localScale = new Vector2(0.0f, 0.0f);

        isLevelTheme = false;
    }

    // Update is called once per frame
    void Update()
    {

        //Open the box!
        presentTop.rectTransform.position = Vector2.Lerp(topStart, topStart + new Vector2(0.0f, 100.0f), topPresentT);
        presentBottom.rectTransform.position = Vector2.Lerp(bottomStart, bottomStart + new Vector2(0.0f, -200.0f), bottomPresentT);

        topPresentT += Time.deltaTime;
        bottomPresentT += Time.deltaTime;

        //The new skin icon grows when the box is done
        if (topPresentT > 1.0f && skinIconT < 1.0f)
        {
            skinIconT += Time.deltaTime;

            //Stuff shall grow here
            newSkinIcon.rectTransform.sizeDelta = Vector2.Lerp(new Vector2(0.0f,0.0f), new Vector2(1.0f, 1.0f), skinIconT);
            newSkinName.rectTransform.localScale = Vector2.Lerp(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), skinIconT);
            tryItButton.transform.localScale = Vector2.Lerp(new Vector2(0.0f, 0.0f), new Vector2(1.0f, 1.0f), skinIconT);
        
            //When done... (Should occur only once)
            if (skinIconT >= 1.0f)
            {
                tryItButton.onClick.AddListener(ClickOnTryIt);
                newSkinName.gameObject.SetActive(true);
            }
        }

        

    }

    //Call one of these upon opening the menu
    public void SetThemeIcon(string name, Skins.Themes theme)
    {
        newSkinIcon = themeIcons[(int)theme];
        newSkinName.text = name;
        isLevelTheme = true;
        Skins.levelTheme = theme;
    }
    public void SetSnakeIcon(string name, Skins.SnakeSkins skin)
    {
        newSkinIcon = snakeIcons[(int)skin];
        newSkinName.text = name;
        Skins.snakeSkin = skin;
    }

    //Player clicks continue button to try it out!
    void ClickOnTryIt()
    {

        if (isLevelTheme)
        {
            Skins.randomTheme = false;
        }
        else
        {
            Skins.randomSkin = false;

            //Change the player skin if not randomizing
            GameObject.Find("Player").GetComponent<Snake>().ChangeSnakeSkin();
        }


        //Save game so skin is here for good.
        GlobalStats.Save();
        
        //Get rid of this screen.
        if (isLevelTheme) SceneManager.LoadScene(0);
        else
        {
            GlobalStats.hud.ChangeMenuMode(1);
            gameObject.SetActive(false);
        }
    }
}
