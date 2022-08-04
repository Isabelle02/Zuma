using System;
using UnityEngine;

[Serializable]
public class PlayerData : BaseData
{
    public Vector3 Position;
    
    public override BaseEntity CreateEntity(IWorld world)
    {
        return new PlayerEntity(this);
    }
}