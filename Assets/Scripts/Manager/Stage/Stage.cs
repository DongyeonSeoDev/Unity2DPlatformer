using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Stage
{
    public GameObject stageObject;
    public GameObject stagePrefab;
    public GameObject stageSign;
    public GameObject lockDoorSign;
    public SpriteRenderer doorSignSpriteRender;
    public Text stageText;
    public Vector3 stageStartPosition;
    public int stageNumber;
    public float highScore;
    public bool isStageClear;
    public bool isAnimationPlay;
}
