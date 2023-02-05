using UnityEngine;

public class MoveCheck : MonoBehaviour
{
    private Vector3 previousPosition;

    [SerializeField] private AudioSource diggingSound;

    private void Start()
    {
        previousPosition = transform.position;
    }

    private void Update()
    {
        Vector3 currentPosition = transform.position;

        if (currentPosition != previousPosition)
        {
            diggingSound.UnPause();
        }
        else
        {
            diggingSound.Pause();
        }

        previousPosition = currentPosition;
    }
}
