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

    private Color[] colors = new Color[5];

    private void Start()
    {
        Physics.IgnoreCollision(Player.Instance.GetComponent<Collider>(), GetComponent<Collider>());
        _meshRenderer = GetComponent<MeshRenderer>();
        _followPath = GetComponent<FollowPath>();
        SetColors();
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
        }
        if (other.gameObject.CompareTag("RoadEnd"))
        {
            _meshRenderer.enabled = false;
            _meshRenderer.material.color = colors[Random.Range(0,5)];
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
        _followPath.speed = Random.Range(3f, 4f);
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

    private void SetColors()
    {
        colors[0] = Color.cyan;
        colors[1] = Color.yellow;
        colors[2] = Color.magenta;
        colors[3] = Color.green;
        colors[4] = Color.red;
        _meshRenderer.material.color = colors[Random.Range(0, 5)];
    }
}
