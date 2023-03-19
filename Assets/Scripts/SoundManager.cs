using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClip axeAudioClip;
    [SerializeField] private AudioClip pickaxeAudioClip;
    [SerializeField] private AudioClip scytheAudioClip;
    [SerializeField] private AudioClip dynamiteAudioClip;
    [SerializeField] private AudioClip vehicleAudioClip;

    [SerializeField] private AudioSource spaceboiAudioSource;


    public void PlayAudioClip(Inventory.Tool tool) {
        Debug.Log("Audio Clip Playing Now");
        switch (tool) {
            case Inventory.Tool.Axe:
                spaceboiAudioSource.clip = axeAudioClip;
                spaceboiAudioSource.Play();
                break;
            case Inventory.Tool.Pickaxe:
                spaceboiAudioSource.clip = pickaxeAudioClip;
                spaceboiAudioSource.Play();
                break;
            case Inventory.Tool.Scythe:
                spaceboiAudioSource.clip = scytheAudioClip;
                spaceboiAudioSource.Play();
                break;
            case Inventory.Tool.Dynamite:
                spaceboiAudioSource.clip = dynamiteAudioClip;
                spaceboiAudioSource.Play();
                break;
            case Inventory.Tool.ChainsawDozer:
                break;
            case Inventory.Tool.Harvester:
                break;
        }
    }
}
