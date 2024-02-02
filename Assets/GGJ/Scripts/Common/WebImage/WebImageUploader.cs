using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace GGJ.Common.WebImage
{
    public class WebImageUploader : MonoBehaviour
    {
        //private const string host = "http://127.0.0.1:8787";
        private const string host = "https://fukuwarai-proxy-workers.sashimi-shimi-sashimi.workers.dev";
        private string uploadPath = "/upload";
        private string basicKey = "sashimi";
        private string basicPass = "daisuki";

        public async UniTask Upload(Texture2D tex)
        {
            WWWForm form = new WWWForm();
            form.AddField("requireSignedURLs", "true");
            form.AddBinaryData("file", tex.EncodeToJPG(), "result.jpg", "image/jpg");

            var url = host + uploadPath;
            using (var request = UnityWebRequest.Post(url, form))
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
                        Debug.Log("成功");
                        break;
                }
            }
        }
    }
}
