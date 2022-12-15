using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FPSCounter : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public float updateInterval;
    int fps = 0;

    void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(F());
    }
    IEnumerator F()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        textMesh.text = fps.ToString();
        yield return new WaitForSeconds(1);
        StartCoroutine(F());
    }
}