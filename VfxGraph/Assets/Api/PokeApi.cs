using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;
using System;
using UnityEditor;

public class PokeApi : MonoBehaviour
{
    public TMPro.TMP_InputField InputField;
    public Image ShinyPicture;
    private AudioSource Audiothingy;


    void Start()
    {
        Audiothingy = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if(InputField.text.Length > 0)
            {
                StartCoroutine(CheckApi());
            }
        }
    }

    private IEnumerator CheckApi()
    {
        PokeData data;
        using (UnityWebRequest request = UnityWebRequest.Get("https://pokeapi.co/api/v2/pokemon/" + InputField.text))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            data = JsonUtility.FromJson<PokeData>(request.downloadHandler.text);
            Debug.Log(data.sprites.front_shiny);
        }
        if (data != null)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(data.sprites.front_shiny))
            {
                yield return request.SendWebRequest();
                if(request.result != UnityWebRequest.Result.Success)
                {
                    yield break;
                }
                Texture2D image = DownloadHandlerTexture.GetContent(request);
                ShinyPicture.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
            }

            using (UnityWebRequest request = UnityWebRequestMultimedia.GetAudioClip(data.cries.latest, AudioType.OGGVORBIS))
            {
                yield return request.SendWebRequest();
                if (request.result != UnityWebRequest.Result.Success)
                {
                    yield break;
                }
                AudioClip clip = DownloadHandlerAudioClip.GetContent(request);
                Audiothingy.clip = clip;
                Audiothingy.Play();
            }
        }
    }

    [Serializable]
    public class PokeData
    {
        public string name;
        public PokeSprite sprites;
        public PokeCry cries;
    }

    [Serializable]
    public class PokeSprite
    {
        public string front_shiny;
    }

    [Serializable]
    public class PokeCry
    {
        public string latest;
    }
}
