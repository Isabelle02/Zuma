public class PlayerEntity : BaseEntity<PlayerData>
{
    private PlayerView _playerView;
    
    public PlayerEntity(PlayerData data) : base(data)
    {
        _playerView = Recycler<PlayerView>.GetObj();
        _playerView.transform.position = data.Position;
    }
}