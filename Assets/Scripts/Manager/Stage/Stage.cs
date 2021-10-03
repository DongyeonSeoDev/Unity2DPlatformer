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

[System.Serializable]
public class StageSaveData
{
    public float highScore;
    public bool isStageClear;
    public bool isAnimationPlay;

    public StageSaveData(float highScore, bool isStageClear, bool isAnimationPlay)
    {
        this.highScore = highScore;
        this.isStageClear = isStageClear;
        this.isAnimationPlay = isAnimationPlay;
    }

    public static StageSaveData FromStageToStageSaveData(Stage stage)
    {
        return new StageSaveData(stage.highScore, stage.isStageClear, stage.isAnimationPlay);
    }

    public static void FromStageSaveDataToStage(Stage stage, StageSaveData stageSaveData)
    {
        stage.highScore = stageSaveData.highScore;
        stage.isStageClear = stageSaveData.isStageClear;
        stage.isAnimationPlay = stageSaveData.isAnimationPlay;
    }
}