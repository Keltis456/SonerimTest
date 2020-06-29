using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(BoxCollider))]
internal class HotObjectController : MonoBehaviour
{
    private string title;
    private string description;
    private Sprite image;
    
    private Color startcolor;

    public void Initialize(string title,string description,string imageUrl)
    {
        this.title = title;
        this.description = description;
        StartCoroutine(GetTexture(imageUrl));
    }

    private IEnumerator GetTexture(string url) {
        var www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (!www.isNetworkError && !www.isHttpError)
        {
            var myTexture = ((DownloadHandlerTexture) www.downloadHandler).texture;
            image = Sprite.Create(myTexture,new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    private void OnMouseEnter()
    {
        startcolor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.yellow;
        
        PopUpController.Instance.Show(title,description, image);
    }

    private void OnMouseExit()
    {
        GetComponent<Renderer>().material.color = startcolor;
        
        PopUpController.Instance.Hide();
    }
}