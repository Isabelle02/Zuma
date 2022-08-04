public interface IBallPool : IActor
{
    public void Boost(float timeScale);
    public void RemoveBall(BallView ball);
    public void Destroy();
}