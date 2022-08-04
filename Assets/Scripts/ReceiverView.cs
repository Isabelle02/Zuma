using UnityEngine;

public class ReceiverView : MonoBehaviour
{
    public ReceiverEntity ReceiverEntity;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BallView>(out var ball))
        {
            ReceiverEntity.OnBallReceived(ball);
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}