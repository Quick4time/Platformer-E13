using UnityEngine;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range (0f, 1f)] // Расстояние между двумя значениями которые можно указывать в инспекторе либо Random.Range
    public float volume = 0.7f; // Значение Громкости. 
    [Range(0.5f, 1.5f)]
    public float pitch = 1f; // изменение частоты звука.

    [Range (0.1f, 0.5f)]
    public float randomVolume = 0.1f;
    [Range (0.1f, 0.5f)]
    public float randomPitch = 0.1f;

    public bool loop;

    private AudioSource source;

    public void SetSourse (AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }

    public void Play ()
    {
        source.volume = volume * (1 + Random.Range(-randomVolume / 2f, randomVolume / 2f));
        source.pitch = pitch * (1 + Random.Range(-randomPitch / 2f, randomPitch / 2f));
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }


}
/// <summary>
/// Для добавления звука в игру, для начала в переменные добавляем 
/// private AudioManager audioManager;
/// Далее в void Start'е() добавляем проверку на надичие АудиоМенеджера в сцене
/// {
///     audioManager = AudioManager.instance; // синглтон.
///     if (audioManager == null)
///     {
///         Debug.LogError ("No AudioManager on scene!!!");
///     }
/// }
/// 
/// в нужной строке метода audioManager.PlaySound(тут пишем название аудиозаписи из Inspector'a AudioManagera в сцене).
/// </summary>


public class AudioManager : MonoBehaviour {

    public static AudioManager instance;

    [SerializeField]
    Sound[] sounds;
	
    void Awake ()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    void Start ()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].SetSourse(_go.AddComponent<AudioSource>());
        }
        PlaySound("Music");
    }

    public void PlaySound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Play();
                return;
            }
        }
    }
           
        public void StopSound(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name == _name)
            {
                sounds[i].Stop();
                return;
            }
        }
        // нет звука под _name
        Debug.LogWarning ("AudioManager: Sound not found in list, "+ _name);
    }
}
