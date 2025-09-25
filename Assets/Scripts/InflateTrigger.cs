using UnityEngine;

public class InflateTrigger : MonoBehaviour
{
    public Bubble triggeredBubble;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TriggerInflate()
    {
        triggeredBubble.Inflate();
    }
}
