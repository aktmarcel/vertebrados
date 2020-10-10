using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer instance;
    public  AudioClip[] clips;
    public static AudioSource audioSource;
    public static float musicVolume = 0.5f;

    void Start(){
        //instance = this;//instancia essa classe para ser estatica para todas as telas
        audioSource = this.GetComponent<AudioSource>();
        audioSource.loop = false;
    }

    void Update() {
        if(!audioSource.isPlaying){
            audioSource.clip = GetRandomClip();
            audioSource.Play();
        }
        audioSource.volume = musicVolume;
    }

    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public static MusicPlayer Instance {
        get { return instance; }
    }

    private AudioClip GetRandomClip(){
        return clips[Random.Range(0, clips.Length)];
    }

    public void SetVolume(float vol){
        musicVolume = vol;
    }
}
