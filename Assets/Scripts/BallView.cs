using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class BallView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private Tween _tween;
    
    public void SetColor(Color color)
    {
        _spriteRenderer.color = color;
    }

    public void Move(IEnumerable<Vector3> points, float duration, float timeScale)
    {
        var pathPoints = points.Select(t => t - Vector3.forward / 2).ToList();
        _tween = transform.DOPath(pathPoints.ToArray(), duration).SetEase(Ease.Linear);
        
        _tween.timeScale = timeScale;
    }

    public void Boost(float timeScale)
    {
        _tween.timeScale = timeScale;
    }

    public void Dispose()
    {
        _tween.timeScale = 1;
        _tween?.Kill();
    }
}