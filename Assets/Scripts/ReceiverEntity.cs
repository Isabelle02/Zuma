using System;

public class ReceiverEntity : BaseEntity<ReceiverData>, IBallReceiver
{
    private ReceiverView _receiverView;
    
    public event Action<BallView> BallReceived;
    
    public void Init(CurveEntity curve)
    {
        _receiverView = Recycler<ReceiverView>.GetObj();
        _receiverView.transform.position = curve.TargetPosition;
        _receiverView.ReceiverEntity = this;
    }
    
    public ReceiverEntity(ReceiverData data) : base(data)
    {
    }

    public void OnBallReceived(BallView ball)
    {
        BallReceived?.Invoke(ball);
    }
    
    public void Destroy()
    {
        _receiverView.Destroy();
    }
}