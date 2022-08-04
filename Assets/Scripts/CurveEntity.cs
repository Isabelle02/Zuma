using System.Collections.Generic;
using System.Linq;
using BansheeGz.BGSpline.Curve;
using UnityEngine;

public class CurveEntity : BaseEntity<CurveData>
{
    private BGCurve _curveView;

    public float Duration => Data.Duration;
    
    public int ColorCount => Data.ColorCount;
    
    public int LimitBallCount => Data.LimitBallCount;

    public Vector3 StartBoardPosition => Data.StartBoardPosition;

    public Vector3 TargetPosition => GetPoints().LastOrDefault() - Vector3.forward;

    public Vector3 StartPosition => GetPoints().FirstOrDefault() - Vector3.forward;

    public List<Vector3> GetPoints()
    {
        var line = _curveView.gameObject.GetComponent<LineRenderer>();
        var points = new List<Vector3>();
        for (var i = 0; i < line.positionCount; i++)
        {
            points.Add(line.GetPosition(i));
        }

        return points;
    }

    public CurveEntity(CurveData data, IWorld world) : base(data)
    {
        _curveView = Recycler<BGCurve>.GetObj();
        foreach (var p in data.Points)
            _curveView.AddPoint(_curveView.CreatePointFromWorldPosition(
                p.Position, p.ControlType, 
                p.ControlFirst, p.ControlSecond));

        var ballPoolData = new BallPoolData();
        var ballPoolEntity = world.CreateNewObject(ballPoolData) as BallPoolEntity;
        ballPoolEntity?.Init(this);

        var receiverData = new ReceiverData();
        var receiverEntity = world.CreateNewObject(receiverData) as ReceiverEntity;
        receiverEntity?.Init(this);
    }
}