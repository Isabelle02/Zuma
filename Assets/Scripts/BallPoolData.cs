public class BallPoolData : BaseData
{
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new BallPoolEntity(this);
    }
}