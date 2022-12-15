using System.Collections;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private FollowPath _followPath;
    private ShopManager _shopManager;

    [SerializeField] 
    private GameObject[] shops;
    [SerializeField]
    private GameObject roadEnd;
    public Transform _spawnPoint;

    public ParticleSystem _angryParticle;
    public ParticleSystem _happyParticle;
    private SkinnedMeshRenderer _meshRenderer;
    private Animator _animator;

    public int buyChance = 50;
    public int random;
    public int randomShop;
    public bool isFull;

    private Color[] colors = new Color[5];

    private void Start()
    {
        Physics.IgnoreCollision(Player.Instance.GetComponent<Collider>(), GetComponent<Collider>());
        _meshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _followPath = GetComponent<FollowPath>();
        SetColors();
        random = Random.Range(0, 100);
    }

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _animator.SetInteger("WalkType", Random.Range(1, 4));
        StartCoroutine(BuyOrNot());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator RoadEndCheck()
    {
        while (true)
        {
            yield return ArifHelpers.GetWait(0.1f);

            if (ArifHelpers.DistanceCollider(this.gameObject, roadEnd, 3f))
            {
                _meshRenderer.enabled = false;
                _meshRenderer.material.color = colors[Random.Range(0, 5)];
                _followPath.speed = 20;
                random = Random.Range(0, 100);
                _followPath.GoTo(0);
                isFull = false;
                StartCoroutine(RespawnCustomer());
                yield break;
            }
        }
    }

    private IEnumerator ShopDistanceCheck()
    {
        while (true)
        {
            yield return ArifHelpers.GetWait(0.1f);

            foreach (var go in shops)
            {
                if (ArifHelpers.DistanceCollider(this.gameObject, go, 4.5f))
                {
                    _shopManager = go.GetComponentInParent<ShopManager>();
                    if (_shopManager.Sell())
                    {
                        _animator.SetBool("IsShopping", true);
                        yield return ArifHelpers.GetWait(2.5f);
                        _happyParticle.Play();
                        _animator.SetBool("IsShopping", false);
                    }
                    else
                    {
                        _animator.SetBool("IsShopping", true);
                        yield return ArifHelpers.GetWait(2.5f);
                        _angryParticle.Play();
                        _animator.SetBool("IsShopping", false);
                    }

                    isFull = true;
                    yield break;
                }
            }
        }
    }

    private IEnumerator BuyOrNot()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (random <= buyChance)
            {
                randomShop = Random.Range(3, 6);
                _followPath.GoTo(randomShop);
                StartCoroutine(Shopping());
                yield break;
            }
            else
            {
                StartCoroutine(RoadEndCheck());
                _followPath.GoTo(2);
                yield break;
            }
        }
    }
    
    private IEnumerator RespawnCustomer()
    {
        yield return ArifHelpers.GetWait(Random.Range(3, 10));
        _animator.SetInteger("WalkType", Random.Range(1, 4));
        transform.rotation = _spawnPoint.rotation;
        _followPath.speed = Random.Range(1.75f, 2.25f);
        _meshRenderer.enabled = true;
        StartCoroutine(BuyOrNot());
    }

    private IEnumerator Shopping()
    {
        StartCoroutine(ShopDistanceCheck());

        while (true)
        {
            yield return ArifHelpers.GetWait(0.1f);
            if (isFull)
            {
                _followPath.GoTo(2);
                StartCoroutine(RoadEndCheck());
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
