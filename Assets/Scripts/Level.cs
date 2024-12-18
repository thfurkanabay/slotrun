using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level")]
public class Level : ScriptableObject
{
    public string levelName;
    public int entryFee;
    public int goalObjectAmount;
    public float levelSpeed;
    public int obstacleCount;
    public int rewardAmount;
    public bool isComplete;
}
