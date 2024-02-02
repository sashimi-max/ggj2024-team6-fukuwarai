using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GGJ.Game.WebImage
{
    [CustomEditor(typeof(GameCanvasWebUploader))]
    public class GameCanvasWebUploadTester : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameCanvasWebUploader gameCanvasWebUploader = target as GameCanvasWebUploader;

            if (GUILayout.Button("WebUploader テスト送信"))
            {
                gameCanvasWebUploader.WebUploadCurrentCanvas().Forget();
            }
        }
    }
}
