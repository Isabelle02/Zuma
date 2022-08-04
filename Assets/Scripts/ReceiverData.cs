public class ReceiverData : BaseData
{
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new ReceiverEntity(this);
    }
}