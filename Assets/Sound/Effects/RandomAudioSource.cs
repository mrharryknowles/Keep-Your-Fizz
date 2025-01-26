using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomAudioSource : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    public void Play() {
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}
