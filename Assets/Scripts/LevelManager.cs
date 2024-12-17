using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Image levelGoalImage;
    public int levelDifficulty;
    public Slider levelSlider;
    public bool isCurrentLevelComplete;

    /*public bool IsCurrentGoalObject(bool isCurrentGoalObject)
    {
        if ()
        {
            return true;

        }
    }*/


    public void PlaySFX() { }

    public void LevelLose()
    {

    }
    public void LevelWin()
    {

    }
}