using UnityEngine.Audio;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    public static SoundManager instance;
    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.GetComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }
    void Start()
    {
        // Play theme here
    }
    public void Play (string name_sound)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name_sound);
        if (s is null)
            return;
        s.source.Play();
    }
}
