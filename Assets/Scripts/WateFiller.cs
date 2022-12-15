using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WateFiller : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private ParticleSystem _particles;
    [SerializeField] private float distance = 1;

    private Collector _collector;
    private Vector3 _position;

    private void Start()
    {
        _position = transform.position;
        _collector = _player.GetComponent<Collector>();
        StartCoroutine(FillWater());
    }

    private IEnumerator FillWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (ArifHelpers.DistanceCollider(this.gameObject, _player, distance))
            {
                Vector3 v3 = new Vector3(_position.x, _position.y + 0.1f, _position.z);
                Tween tw = this.transform.DOShakeScale(0.3f,0.1f);
                tw.OnComplete(() => { if (_collector.waterLiter < 100) { _collector.waterLiter += 10; }; });
                _particles.Play();
            }
        }
    }
}
