using UnityEngine;

public class Gameplay : MonoBehaviour
{
    private BaseWorld _world;

    public void Start()
    {
        Init(0);
    }

    public void Init(int i)
    {
        var level = LevelManager.LoadLevel(i);
        
        var wd = new WorldData();
        wd.Data.AddRange(level.Curves);
        wd.Data.Add(level.Player);
        
        _world = new BaseWorld();

        var ballDistributionSystem = new BallDistributionSystem();
        
        _world.Activate(wd, ballDistributionSystem);
    }

    public void Update()
    {
        
    }
}
