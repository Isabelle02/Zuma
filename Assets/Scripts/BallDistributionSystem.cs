using System.Collections.Generic;

public class BallDistributionSystem : BaseSystem<IBallPool, IBallReceiver>
{
    private readonly List<IBallPool> _ballPools = new List<IBallPool>();
    private readonly List<IBallReceiver> _ballReceivers = new List<IBallReceiver>();

    private float _boost = 1;

    protected override void AddActor(IBallPool actor)
    {
        _ballPools.Add(actor);
    }

    protected override void RemoveActor(IBallPool actor)
    {
        actor.Destroy();
        _ballPools.Remove(actor);
    }
    
    protected override void AddActor(IBallReceiver actor)
    {
        actor.BallReceived += OnBallReceived;
        _ballReceivers.Add(actor);
    }

    protected override void RemoveActor(IBallReceiver actor)
    {
        actor.Destroy();
        _ballReceivers.Remove(actor);
    }

    private void OnBallReceived(BallView ball)
    {
        _boost *= 8;
        foreach (var bp in _ballPools)
        {
            bp.Boost(_boost);
            bp.RemoveBall(ball);
        }

        Recycler<BallView>.ReleaseObj(ball);
    }
}