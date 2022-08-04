using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CurveData : BaseData
{
    public List<PointData> Points = new List<PointData>();
    public float Duration;
    public int ColorCount;
    public int LimitBallCount;
    public Vector3 StartBoardPosition;
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new CurveEntity(this, world);
    }
}