using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private FollowPath _followPath;
    private ShopManager _shopManager;
    public Transform _spawnPoint;

    private MeshRenderer _meshRenderer;

    public int buyChance = 50;
    public int random;
    public bool isFull;

    private void Start()
    {
        Physics.IgnoreCollision(Player.Instance.GetComponent<Collider>(), GetComponent<Collider>());

        _meshRenderer = GetComponent<MeshRenderer>();
        _followPath = GetComponent<FollowPath>();
        random = Random.Range(0, 100);
        StartCoroutine(BuyOrNot());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Shop"))
        {
            _shopManager = other.GetComponentInParent<ShopManager>();
            _shopManager.Sell();
            isFull = true;
            //Give bag to hand
        }
        if (other.gameObject.CompareTag("RoadEnd"))
        {
            _meshRenderer.enabled = false;
            _followPath.speed = 20;
            random = Random.Range(0, 100);
            _followPath.GoTo(0);
            isFull = false;
            StartCoroutine(RespawnCustomer());
        }
    }

    private IEnumerator BuyOrNot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (random <= buyChance)
            {
                _followPath.GoTo(3);
                StartCoroutine(Shopping());
                yield break;
            }
            else
            {
                _followPath.GoTo(2);
                yield break;
            }
        }
    }
    
    private IEnumerator RespawnCustomer()
    {
        yield return new WaitForSeconds(Random.Range(5, 10));
        transform.rotation = _spawnPoint.rotation;
        _followPath.speed = Random.Range(4.5f, 5.5f);
        _meshRenderer.enabled = true;
        StartCoroutine(BuyOrNot());
    }

    private IEnumerator Shopping()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            if (isFull)
            {
                yield return new WaitForSeconds(2f);
                _followPath.GoTo(2);
                yield break;
            }
        }
    }
}
