using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewChapter", menuName = "Game/Chapter")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public string chapterDescription;
    public Sprite chapterBackGroundSprite;
    public Sprite chapterGroundSprite;
    public GameObject chapterCharacter;
    public List<Sprite> goalObjectImagelist;
    public AudioClip chapterMX;
    public List<Level> levels;

}