using UnityEngine;

public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float timeFrameMin;
    [SerializeField] private float timeFrameMax;
    [SerializeField] private float randomDelay;
    [SerializeField] private float pitchMin;
    [SerializeField] private float pitchMax;

    private void Start()
    {
        InvokeRepeating("PlayRandomClip", timeFrameMin, timeFrameMax);
    }

    private void PlayRandomClip()
    {
        int randomIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomIndex];
        audioSource.pitch = Random.Range(pitchMin, pitchMax);
        audioSource.PlayDelayed(Random.Range(0, randomDelay));
        Invoke("PlayRandomClip", Random.Range(timeFrameMin, timeFrameMax));
    }
}
