using UnityEngine;
using UnityEngine.UI;

public class HeartContainer : MonoBehaviour
{
    [SerializeField] private Sprite GoodHeartSprite;

    [SerializeField] private Sprite BadHeartSprite;
    [SerializeField] private Image HeartImage;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip heartAudio;


    public void SetSprite(bool val)
    {
        HeartImage.sprite = val ? GoodHeartSprite : BadHeartSprite;
        if (!val) sfxSource.PlayOneShot(heartAudio);
    }
}