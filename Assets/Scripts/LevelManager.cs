using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Image levelGoalImage;
    public int levelDifficulty;
    public Slider levelSlider;

    private void Start()
    {
        // Choose level goal object randomly and assign. And choose level goal amount and assign the slider rate.

    }

    // Level prefabını spawn etme
    private void InitializeLevel()
    {

        //var levelInstance = Instantiate(levelPrefab, currentLevel.spawnPoint.position, Quaternion.identity);
        //var levelController = levelInstance.GetComponent<LevelController>();

        // Level ayarlarını ilet
        //levelController.InitializeLevel(currentLevel);
    }
}