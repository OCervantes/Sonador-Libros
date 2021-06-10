using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class DefinitionAudioLoader : MonoBehaviour
{
    public AudioClip[] audioClips;
    public string m_recoveredVocabWord;
    public GameObject audioHolder;
    public AudioSource audio;
    
    // Start is called before the first frame update
    void Start()
    {
        //Creates a new GameObject were the sprite is going to be added.
        audioHolder = new GameObject("Audio");
        //Initialize image position.
        audioHolder.transform.position = new Vector3(-5,0,0);
        audio = audioHolder.AddComponent<AudioSource>();
        //Addressables.LoadAssetsAsync<Sprite>("vocabulary", null).Completed += OnSpritesLoaded;
    }

    private void OnAudiosLoaded(AsyncOperationHandle<IList<AudioClip>> result)
	{
		if (result.Status == AsyncOperationStatus.Succeeded)
		{
			if (result.Result != null && result.Result.Count > 0)
			{
				IList<AudioClip> list = result.Result;
				audioClips = new AudioClip[list.Count];
				list.CopyTo(audioClips, 0);
				Debug.Log("Result is " + list.Count);
                AssignAudiosToGameobject();
			}
			else if (result.Result == null)
			{
				Debug.Log("Result is null");
			} else
			{
				Debug.Log("Result is 0");
			}
			//Addressables.Release<IList<Sprite>>(result);
		}
		else if (result.Status == AsyncOperationStatus.Failed)
		{
			Debug.Log("List of Audio failed to load.");
		}
	}

    public void InstantiateAudio(string recoveredVocabWord)
    {
        m_recoveredVocabWord = recoveredVocabWord;
        Addressables.LoadAssetsAsync<AudioClip>("definitions", null).Completed += OnAudiosLoaded;
    }

    public void AssignAudiosToGameobject()
    {
        /*for each  sprites from the addressable assets group (vocabulary)
        compare their names with the current word that the user has to guess,
        if they match then instantiate the object (assign the sprite as child of 
        the gameobject imageholder) */
        foreach(AudioClip element in audioClips)
        { 
            //Debug.Log("Entre a foreach");
            //Debug.Log(element.name);
            if(element.name == m_recoveredVocabWord)
            {
                Debug.Log("Audio Instantiated");
                audio.clip = element;
                audio.Play();
            }
        }
    }
}
