using UnityEngine;
using UnityEngine.Events;

public class Bubble : MonoBehaviour
{
    public GameObject explodedPrefab;
    public GameObject bubblePrefab;
    public UnityEvent bubbleExplode;
    public UnityEvent bubbleInflate;
    public bool isExploded;
    public AudioSource popSfx;


    private void Start()
    {
        explodedPrefab.SetActive(isExploded);
        bubblePrefab.SetActive(!isExploded);
    }


    public void Explode()
    {
        if (isExploded) return;
        popSfx.pitch = Random.Range(1f, 1.4f);
        explodedPrefab.SetActive(true);
        bubblePrefab.SetActive(false);
        popSfx.Play();
        isExploded = true;
        bubbleExplode?.Invoke();
    }

    public void Inflate()
    {
        if (!isExploded) return;
        explodedPrefab.SetActive(false);
        bubblePrefab.SetActive(true);
        isExploded = false;
        bubbleInflate?.Invoke();
    }
}