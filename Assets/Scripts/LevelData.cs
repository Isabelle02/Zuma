using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LevelData
{
    public List<CurveData> Curves = new List<CurveData>();
    public PlayerData Player;
    public int BallColorCount;
    public int LimitBallCount;
    public Vector3 StartBoardPosition;
}