using System.Collections.Generic;
using UnityEngine;

public class BallPoolEntity : BaseEntity<BallPoolData>, IBallPool
{
    private BallPoolView _ballPoolView;
    private StartBoardView _startBoard;

    private List<Vector3> _points = new List<Vector3>();

    private float _duration;
    private float _boost = 1;
    private int _colorCount;
    private int _limitBallCount;
    private int _currentBallCount;
    private readonly List<BallView> _balls = new List<BallView>();

    private bool _isStart = true;

    public void Init(CurveEntity curve)
    {
        _points.AddRange(curve.GetPoints());
        _duration = curve.Duration;
        _colorCount = curve.ColorCount;
        _limitBallCount = curve.LimitBallCount;

        _startBoard = Recycler<StartBoardView>.GetObj();
        _startBoard.transform.position = curve.StartBoardPosition;
        _startBoard.CollidedStartBoard += () =>
        {
            _isStart = false;
            Boost(_boost);
        };

        _ballPoolView = Recycler<BallPoolView>.GetObj();
        _ballPoolView.transform.position = curve.StartPosition;
        _ballPoolView.BallPoolEntity = this;
    }

    public void RunBall(BallView ball)
    {
        _balls.Add(ball);
        _currentBallCount++;
        
        ball.SetColor(LevelManager.LevelsConfig.BallColors[Random.Range(0, _colorCount)]);
        ball.Move(_points, _duration, _boost);
        
        if (_isStart && _balls.Count > 1)
        {
            foreach (var b in _balls)
                b.Boost(_boost * _duration / 3);
        }
    }
    
    public BallPoolEntity(BallPoolData data) : base(data)
    {
    }

    public bool IsInLimit() => _currentBallCount < _limitBallCount;

    public void Boost(float timeScale)
    {
        _boost = timeScale;
        foreach (var b in _balls)
            b.Boost(timeScale);
    }

    public void RemoveBall(BallView ball)
    {
        if (_balls.Contains(ball))
        {
            ball.Dispose();
            _balls.Remove(ball);
        }
    }

    public void Destroy()
    {
        _ballPoolView.Destroy();
    }
}