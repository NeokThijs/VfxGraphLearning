using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.InputSystem;

public class PokeApi : MonoBehaviour
{
    public TMPro.TMP_InputField InputField;
    public Image Image;

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
        using (UnityWebRequest request = UnityWebRequest.Get("https://pokeapi.co/api/v2/pokemon/" + InputField.text))
        {
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                yield break;
            }

            Pokemon data;
            data = JsonUtility.FromJson<Pokemon>(request.downloadHandler.text);
            Debug.Log(data.name);

        } 
    }

    public class Pokemon
    {
        public string name;
    }
}
