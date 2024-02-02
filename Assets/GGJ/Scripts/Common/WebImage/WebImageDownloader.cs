using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UniRx;
using UnityEngine;
using UnityEngine.Networking;

public class WebImageDownloader : MonoBehaviour
{
    private const string host = "https://fukuwarai-proxy-workers.sashimi-shimi-sashimi.workers.dev";
    private string imageListDownloadPath = "/imageList";
    private string imageDownloadPath = "/image";
    private string basicKey = "sashimi";
    private string basicPass = "daisuki";

    public IObservable<byte[]> OnDownLoadImage => _onDownLoadImage;
    private Subject<byte[]> _onDownLoadImage = new Subject<byte[]>();

    public async UniTask<List<byte[]>> DownloadImageRecentry(int page = 1, int perPage = 10)
    {
        var imageIds = await DownloadImageIdList(page, perPage);
        var blobs = new List<byte[]>();
        foreach (var imageId in imageIds)
        {
            var blob = await DownloadImage(imageId);
            if (blob.Length > 0)
            {
                blobs.Add(blob);
            }
        }
        return blobs;
    }

    public async UniTask<List<string>> DownloadImageIdList(int page = 1, int perPage = 10)
    {
        var queryString = System.Web.HttpUtility.ParseQueryString("");
        queryString.Add("page", page.ToString());
        queryString.Add("perPage", perPage.ToString());

        var url = host + imageListDownloadPath;
        var uriBuilder = new UriBuilder(url)
        {
            Query = queryString.ToString()
        };
        using (var request = UnityWebRequest.Get(uriBuilder.Uri))
        {
            var auth = $"{basicKey}:{basicPass}";
            auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            request.SetRequestHeader("Authorization", "Basic " + auth);
            request.SetRequestHeader("Content-Type", "application/json");
            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log($"サーバとの通信に失敗: {request.error}");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"データの処理中にエラー: {request.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTPのエラー: {request.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log("成功");
                    string json = request.downloadHandler.text;
                    ResponseModel response = JsonUtility.FromJson<ResponseModel>(json);
                    return response.result.images.ToList().Select(x => x.id).ToList();
            }

            return new List<string>();
        }
    }

    public async UniTask<byte[]> DownloadImage(string imageId)
    {
        var url = host + imageDownloadPath + "/" + imageId;
        using (var request = UnityWebRequest.Get(url))
        {
            var auth = $"{basicKey}:{basicPass}";
            auth = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
            request.SetRequestHeader("Authorization", "Basic " + auth);
            await request.SendWebRequest();

            switch (request.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log($"サーバとの通信に失敗: {request.error}");
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"データの処理中にエラー: {request.error}");
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError($"HTTPのエラー: {request.error}");
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log($"成功");
                    _onDownLoadImage.OnNext(request.downloadHandler.data);
                    return request.downloadHandler.data;
            }
        }

        return new byte[0];
    }

    [Serializable]
    private class ResponseModel
    {
        public ResponseResult result;
    }


    [Serializable]
    private class ResponseResult
    {
        public ResponseImages[] images;
    }

    [Serializable]
    private class ResponseImages
    {
        public string id;
    }
}
