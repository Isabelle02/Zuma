using System;

public interface IBallReceiver : IActor
{
    public event Action<BallView> BallReceived; 
    public void Destroy();
}