using EasyTransition;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public TransitionSettings transition;
    public float loadDelay = 1f;

    public void LoadScene(string scene)
    {
        TransitionManager.Instance().Transition(scene, transition, loadDelay);
    }
    public void quit()
    {
        Application.Quit();
    }
}