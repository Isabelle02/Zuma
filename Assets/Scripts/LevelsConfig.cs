using System;
using System.Collections.Generic;
using BansheeGz.BGSpline.Curve;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "LevelsConfig", menuName = "Configs/LevelsConfig")]
public class LevelsConfig : ScriptableObject
{
    [JsonIgnore] public BGCurve BgCurvePrefab;
    
    [JsonIgnore] public List<Color> BallColors = new List<Color>();

    [JsonIgnore] public PlayerView PlayerPrefab;
    
    public List<LevelData> Levels = new List<LevelData>();
}