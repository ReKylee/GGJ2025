using UnityEngine;

public class HeartController : MonoBehaviour
{
    private HeartContainer[] hearts;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        hearts = GetComponentsInChildren<HeartContainer>();
        foreach (var heart in hearts) heart.SetSprite(true);
    }

    public void SetHearts(int hp)
    {
        for (var i = 0; i < hearts.Length; i++) hearts[i].SetSprite(i < hp);
    }
}