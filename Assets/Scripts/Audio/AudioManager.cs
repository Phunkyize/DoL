using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private Sound[] sounds=null;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
            Debug.LogError("More then one AudioManger in scene");
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject _go = new GameObject("Sound_" + i + "_" + sounds[i].name);
            _go.transform.SetParent(this.transform);
            sounds[i].setSource(_go.AddComponent<AudioSource>());
           
        }
    }

    // Start is called before the first frame update
    void Start()
    {
       
        //PlaySound("music");
        
    }

    public void PlaySound(string _name)
    {
        for(int i=0; i < sounds.Length; i++)
        {
            
            if (sounds[i].name == _name)
            {
                
                sounds[i].Play();
                return;
            }
        }

        //no sound found
        Debug.LogError("Sound is missing");

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
    }

    public void StopAll()
    {
        foreach(Sound sound in sounds)
        {
            sound.Stop();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
