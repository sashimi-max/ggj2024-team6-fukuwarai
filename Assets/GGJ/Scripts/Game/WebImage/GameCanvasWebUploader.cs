using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GGJ.Common;
using GGJ.Common.WebImage;
using Cysharp.Threading.Tasks;

namespace GGJ.Game.WebImage
{
    public class GameCanvasWebUploader : SingletonMonoBehaviour<GameCanvasWebUploader>
    {
        protected override bool dontDestroyOnLoad { get { return false; } }

        [SerializeField] WebImageUploader webImageUploader;
        [SerializeField] ScreenShoter screenShoter;
        [SerializeField] RectTransform captureArea;
        [SerializeField] bool enableEditorUpload = false;

        private bool isProgress = false;

        public async UniTask WebUploadCurrentCanvas()
        {
            if (Application.isEditor && !enableEditorUpload) return;
            if (isProgress) return;
            isProgress = true;

            var tex = await screenShoter.TakeScreenShot(captureArea);
            await webImageUploader.Upload(tex);

            isProgress = false;
        }
    }
}
