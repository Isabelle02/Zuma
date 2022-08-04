using System;
using UnityEngine;

public class StartBoardView : MonoBehaviour
{
    public event Action CollidedStartBoard;
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BallView>(out var ball))
        {
            CollidedStartBoard?.Invoke();
        }
    }
}