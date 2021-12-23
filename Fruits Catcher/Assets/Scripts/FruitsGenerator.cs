using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FruitsGenerator : MonoBehaviour
{
    private static FruitsGenerator instance;
    private bool active = false;   // True if the game is active

    [SerializeField]
    private int nbRows = 3;       // Number of rows in this level
    [SerializeField]
    private int nbWaves = 16;     // Number of waves in this level
    private int curWave = 0;
    [SerializeField]
    private float frequency = 1f; // Frequency of the waves
    private float timer;          // Time before the next wave

    private float fruitSpeed = 0.5f;     // Speed of the generated fruits
    private float deltaX;         // Space between each row

    private List<FruitType>[] fruitMat;
    private int nbFruitOnScreen = 0;

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

        LoadLevel(1);
    }

    public static FruitsGenerator Instance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = frequency;
        deltaX = 0.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (curWave < nbWaves)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    NextWave();
                    timer = frequency;
                }
            }
            else if (nbFruitOnScreen == 0)
            {
                GameManager.Instance().EndGame();
                active = false;
            }
        }
    }

    public void LoadLevel(int levelNumber)
    {
        curWave = 0;
        fruitMat = new List<FruitType>[nbWaves];
        for (int i = 0; i < nbWaves; ++i)
        {
            fruitMat[i] = new List<FruitType>();
        }
        Load(levelNumber);
        active = true;
    }

    public void Load(int levelNumber)
    {
        try
        {
            string filename = "Levels/level" + levelNumber;
            string file = Resources.Load<TextAsset>(filename).ToString();
            string[] array = file.Split('\n');

            int j = 0;
            foreach (string line in array)
            {
                string[] fruitList = line.Split('|');

                int i = 0;
                foreach (string fruit in fruitList)
                {
                    if (fruit == "" || i > nbRows - 1)
                        continue;

                    FruitType fruitType;
                    switch(fruit)
                    {
                        case "1":
                            fruitType = FruitType.Apple;
                            break;
                        case "2":
                            fruitType = FruitType.Banana;
                            break;
                        case "3":
                            fruitType = FruitType.Lemon;
                            break;
                        default:
                            fruitType = FruitType.Empty;
                            break;
                    }
                    fruitMat[j].Add(fruitType);
                    ++i;
                }
                ++j;
            }
        }
        catch (Exception e)
        {
            // Directory not found
            Debug.Log(e.Message);
            Application.Quit();
        }
    }

    private void NextWave()
    {
        if (curWave < nbWaves)
        {
            int i = 0;
            foreach(FruitType fruitType in fruitMat[curWave])
            {
                if (fruitType != FruitType.Empty)
                {
                    string filename = "Prefabs/";
                    switch(fruitType)
                    {
                        case FruitType.Apple:
                            filename += "apple";
                            break;
                        case FruitType.Banana:
                            filename += "banana";
                            break;
                        case FruitType.Lemon:
                            filename += "lemon";
                            break;
                    }
                    NewFruit(filename, i);
                }
                ++i;
            }
            ++curWave;
        }
    }

    private void NewFruit(string filename, int row)
    {
        GameObject fruitInstance = Instantiate(Resources.Load(filename)) as GameObject;
        fruitInstance.transform.parent = transform;
        fruitInstance.transform.localPosition = new Vector3(-0.3f + deltaX * row, 0, 0);

        Fruit fruitComponent = fruitInstance.GetComponent<Fruit>();
        fruitComponent.Init(fruitSpeed);

        ++nbFruitOnScreen;
    }

    public void DestroyFruit(Fruit fruit)
    {
        --nbFruitOnScreen;
        Destroy(fruit.gameObject);
    }
}
