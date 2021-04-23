using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;



public class WordImagePlacer : MonoBehaviour
{
    public Sprite[] sprites;
    public string m_recoveredVocabWord;
    public GameObject imageHolder;
    public SpriteRenderer renderer;
    
    // Start is called before the first frame update
    void Start()
    {
        //Creates a new GameObject were the sprite is going to be added.
        imageHolder = new GameObject("Name");
        renderer = imageHolder.AddComponent<SpriteRenderer>();
        //Addressables.LoadAssetsAsync<Sprite>("vocabulary", null).Completed += OnSpritesLoaded;
    }

    private void OnSpritesLoaded(AsyncOperationHandle<IList<Sprite>> result)
	{
		if (result.Status == AsyncOperationStatus.Succeeded)
		{
			if (result.Result != null && result.Result.Count > 0)
			{
				IList<Sprite> list = result.Result;
				sprites = new Sprite[list.Count];
				list.CopyTo(sprites, 0);
				Debug.Log("Result is " + list.Count);
                AssignSpriteToGameobject();
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
			Debug.Log("List of Texture2D failed to load.");
		}
	}

    public void InstantiateImage(string recoveredVocabWord)
    {
        m_recoveredVocabWord = recoveredVocabWord;
        Addressables.LoadAssetsAsync<Sprite>("vocabulary", null).Completed += OnSpritesLoaded;
    }

    public void AssignSpriteToGameobject()
    {
        /*for each  sprites from the addressable assets group (vocabulary)
        compare their names with the current word that the user has to guess,
        if they match then instantiate the object (assign the sprite as child of 
        the gameobject imageholder) */
        foreach(Sprite element in sprites)
        { 
            //Debug.Log("Entre a foreach");
            //Debug.Log(element.name);
            if(element.name == m_recoveredVocabWord)
            {
                Debug.Log("Image Instantiate");
                renderer.sprite = element;
            }
        }
    }
}
