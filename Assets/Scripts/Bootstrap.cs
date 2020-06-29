using System;
using System.Collections;
using System.Linq;
using JsonData;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class Bootstrap : MonoBehaviour
{
    private const string BaseUrl = "https://appdev.virtualviewing.co.uk/developer_test/";
    
    private IEnumerator Start()
    {
        yield return LoadTextFromServer(BaseUrl + "config.json",  response => StartCoroutine(ProcessResponse(response)));
    }

    private static IEnumerator ProcessResponse(string response)
    {
        var responseData = JsonConvert.DeserializeObject<ResponseData>(response);
        
        yield return GetAssetBundle(BaseUrl + GetPlatformBundlePath(responseData),SpawnBundleObjects);
        foreach (var hotObject in responseData.HotObjects)
            GameObject.Find(hotObject.Id)?.AddComponent<HotObjectController>()
                .Initialize(hotObject.Title, hotObject.Description, BaseUrl + hotObject.Image);
    }

    private static void SpawnBundleObjects(AssetBundle bundle)
    {
        foreach (var asset in bundle.LoadAllAssets()) 
            Instantiate(asset);
    }

    private static string GetPlatformBundlePath(ResponseData responseData)
    {
        var bundlePlatform = "Windows";
        #if UNITY_IOS
        bundlePlatform = "iOS";
        #endif
        #if UNITY_ANDROID
        bundlePlatform = "Android";
        #endif
        #if UNITY_WEBGL
        bundlePlatform = "WebGL";
        #endif
        #if UNITY_STANDALONE_WIN
        bundlePlatform = "Windows";
        #endif
        return responseData.AssetBundles.FirstOrDefault((x => x.Platform == bundlePlatform))?.AssetBundle;
    }

    private static IEnumerator GetAssetBundle(string path, Action<AssetBundle> response) {
        using (var request = UnityWebRequestAssetBundle.GetAssetBundle(path))
        {
            yield return request.SendWebRequest();
            if (!request.isNetworkError && !request.isHttpError)
            {
                response(DownloadHandlerAssetBundle.GetContent(request));
            }
            else
            {
                Debug.Log(request.error);
                response(null);
            }
        }
    }
    
    private static IEnumerator LoadTextFromServer(string url, Action<string> response)
    {
        using (var request = UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();
            if (!request.isHttpError && !request.isNetworkError)
            {
                response(request.downloadHandler.text);        
            }
            else
            {
                Debug.LogErrorFormat("error request [{0}, {1}]", url, request.error);
                response(null);
            }
        }
    }
}