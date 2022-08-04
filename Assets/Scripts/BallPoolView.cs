using UnityEngine;

public class BallPoolView : MonoBehaviour
{
    public BallPoolEntity BallPoolEntity;
    
    private void Start()
    {
        RunBall();
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<BallView>(out var ball) && BallPoolEntity.IsInLimit())
        {
            RunBall();
        }
    }

    private void RunBall()
    {
        var ball = Recycler<BallView>.GetObj();
        ball.transform.position = transform.position - Vector3.back / 2;
        BallPoolEntity.RunBall(ball);
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}