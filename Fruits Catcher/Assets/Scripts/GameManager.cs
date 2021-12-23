using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private int curLevel = 1;

    private int nbFruit = 0;
    private int nbGoal = 3;
    private int nbMax = 7;
    private FruitType fruitToCatch = FruitType.Apple;

    private bool levelComplete = false;
    private float timeBeforeNextLevel = 5f;
    private float curTime = 0f;

    [SerializeField]
    private Text fruitDisplay;
    private string typeString;
    [SerializeField]
    private Text endDisplay;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static GameManager Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetFruitToCatch(FruitType.Apple);
    }

    // Update is called once per frame
    void Update()
    {
        if (levelComplete)
        {
            curTime += Time.deltaTime;
        }

        if (curTime > timeBeforeNextLevel)
        {
            NextLevel();
        }
    }

    public void SetFruitToCatch(FruitType type)
    {
        fruitToCatch = type;
        typeString = "";

        switch(type)
        {
            case FruitType.Apple:
                typeString = "Apple";
                break;
            case FruitType.Banana:
                typeString = "Banana";
                break;
            case FruitType.Lemon:
                typeString = "Lemon";
                break;
        }

        UpdateDisplay();
    }

    public void Catch(FruitType type)
    {
        if (type == fruitToCatch)
        {
            ++nbFruit;
        }

        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        fruitDisplay.text = typeString + ": " + nbFruit + "/" + nbGoal;
    }

    public void EndGame()
    {
        levelComplete = true;

        if (nbFruit == nbMax)
        {
            endDisplay.color = new Color(217, 243, 10);
            endDisplay.text = "Perfect!";
        }
        else if (nbFruit > nbGoal)
        {
            endDisplay.color = new Color(16, 224, 85);
            endDisplay.text = "Clear!";
        }
        else
        {
            endDisplay.color = new Color(34, 117, 205);
            endDisplay.text = "Game Over";
        }
    }

    private void NextLevel()
    {
        curTime = 0f;

        levelComplete = false;
        if (curLevel < 2)
        {
            ++curLevel;
            endDisplay.text = "";
            nbFruit = 0;
            SetFruitToCatch(FruitType.Apple);

            FruitsGenerator.Instance().LoadLevel(curLevel);
        }
    }
}
