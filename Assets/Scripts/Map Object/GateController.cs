using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private UIFadeTransition transition;
    [HideInInspector] public bool canTrigger = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canTrigger)
        {
            transition?.Trigger_FadeIn(this);
            Destroy(this);
        }
    }
}