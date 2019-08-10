using UnityEngine;
[System.Serializable]
public class Sound 
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.7f;

    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float randomVolume = 0.1f;

    [Range(0f, 0.5f)]
    public float randomPitch = 0.1f;


    public bool loop = false;

    private AudioSource source;
    
    public void setSource(AudioSource _source)
    {
        this.source = _source;
        this.source.clip = clip;
        this.source.loop = loop;
    }


    public void Play()
    {
        this.source.volume = volume * (1+Random.Range(-randomVolume/2f, randomVolume/2f));
        
        this.source.pitch = pitch*(1+Random.Range(-randomPitch/2f, randomPitch/2f));
        
        this.source.Play();
    }
    
    public void Stop()
    {
        this.source.Stop();
    }
}
