using System;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

[Serializable]
public class PointData
{
    public Vector2 Position;
    public Vector2 ControlFirst;
    public Vector2 ControlSecond;
    public BGCurvePoint.ControlTypeEnum ControlType;
}