using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AbTest : MonoBehaviour
{
    public Transform[] _objects;
    private Vector3 _startpos;
    public float _cycleLength;

    private void Start()
    {
        _startpos = transform.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveandRotate();
            //SequencesandTasks();
        }
    }
    private void MoveandRotate()
    {
        transform.DOMove(new Vector3(5, 1, -5), _cycleLength).SetEase(Ease.InOutSine).SetLoops(-1,LoopType.Yoyo);
        //transform.DOMove(new Vector3(0, 2, -5), _cycleLength * 0.5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        transform.DORotate(new Vector3(0,360,0), _cycleLength * 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart);
    }

    private void SequencesandTasks()
    {
        var sequence = DOTween.Sequence();

        foreach (var obj in _objects)
        {
            sequence.Append(obj.DOMoveX(5, Random.Range(1f, 2f)));
        }

        sequence.OnComplete(() =>
        {
            foreach (var obj in _objects)
            {
                obj.DOScale(Vector3.zero, 1f).SetEase(Ease.InOutSine);
            }
        });
    }
}
