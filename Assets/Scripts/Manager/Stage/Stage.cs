using UnityEngine;

[System.Serializable]
public class Stage
{
    public GameObject stageObject;
    public GameObject stagePrefab;
    public GameObject stageSign;
    public SpriteRenderer doorSignSpriteRender;
    public Vector3 stageStartPosition;
    public int stageNumber;
    public bool isStageClear;
}