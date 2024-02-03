﻿using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;

namespace GGJ.Common.WebImage
{
    [CustomEditor(typeof(WebImageDownloader))]
    public class WebImageDownloadTester : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            WebImageDownloader webImageDownloader = target as WebImageDownloader;

            if (GUILayout.Button("画像リスト テストダウンロード"))
            {
                webImageDownloader.DownloadImageIdList(CancellationToken.None).Forget();
            }

            if (GUILayout.Button("画像 テストダウンロード"))
            {
                webImageDownloader.DownloadImage(CancellationToken.None,"e55bae81-5e1c-4e0c-b190-057f61de8b01").Forget();
            }
        }
    }
}
