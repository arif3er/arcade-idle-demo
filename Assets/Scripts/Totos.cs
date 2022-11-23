using UnityEngine;

public class Totos : MonoBehaviour
{
    private FollowPath _followPath;
    

    void Start()
    {
        _followPath = GetComponent<FollowPath>();
        InvokeRepeating("GoToRandomWP", 1, 5);
    }

    private void GoToRandomWP()
    {
        int i = Random.Range(0, WPManager.Instance.waypoints.Length);
        _followPath.GoTo(i);
        Debug.Log(name + " is going to " + i);
    }
}
