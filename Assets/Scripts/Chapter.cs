using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewChapter", menuName = "Game/Chapter")]
public class Chapter : ScriptableObject
{
    public string chapterName;
    public string chapterDescription;
    public Sprite chapterBackGroundSprite;
    public Sprite chapterGroundSprite;
    public GameObject chapterCharacter;

    public List<Level> levels;
}