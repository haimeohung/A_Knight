using UnityEngine.Audio;
using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public static Hashtable sounds = new Hashtable();

    [SerializeField] private SoundList soundList;
    [Range(0f, 1f)] public float globalVolume = 1f;
    float oldGlobalVolume = -1f;

    private void Awake()
    {
        if (instance is null)
        {
            instance = this;
            foreach (Sound s in soundList.list)
            {
                oldGlobalVolume = globalVolume;
                GameObject g = new GameObject(s.name, typeof(AudioSource));
                AudioSource audioSource = g.GetComponent<AudioSource>();
                audioSource.clip = s.clip;
                audioSource.volume = s.volume * globalVolume;
                audioSource.pitch = s.pitch;
                audioSource.loop = s.loop;
                audioSource.Stop();
                g.transform.SetParent(transform);
                sounds.Add(s.name, audioSource);
            }
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (oldGlobalVolume != globalVolume)
        {
            foreach (Object item in sounds.Values)
            {
                AudioSource s = item as AudioSource;
                if (!s.Equals(null))
                    s.volume = s.volume / oldGlobalVolume * globalVolume;
            }
            oldGlobalVolume = globalVolume;
        }
    }

    public void Play(string name)
    {
        AudioSource s = sounds[name] as AudioSource;
        if (s.Equals(null))
            Debug.Log("Âm thanh chưa đưuọc khởi tạo");
        else
        {
            s.Stop();
            s.Play();
        }
    }
    public void Stop(string name) => (sounds[name] as AudioSource)?.Stop();
    public void Pause(string name) => (sounds[name] as AudioSource)?.Pause();
    public void UnPause(string name) => (sounds[name] as AudioSource)?.UnPause();
}
