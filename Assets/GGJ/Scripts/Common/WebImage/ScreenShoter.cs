using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GGJ.Common.WebImage
{
    public class ScreenShoter : MonoBehaviour
    {
        public async UniTask<Texture2D> TakeScreenShot(RectTransform rect)
        {
            // 描画が完了してからでないとエラーが出る
            await UniTask.WaitForEndOfFrame(this);

            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
#if UNITY_EDITOR
            string[] res = UnityStats.screenRes.Split('x');
            screenWidth = int.Parse(res[0]);
            screenHeight = int.Parse(res[1]);
#endif
            var corners = new Vector3[4];
            rect.GetWorldCorners(corners);
            corners[0] = RectTransformUtility.WorldToScreenPoint(Camera.main, corners[0]);
            var _s = screenWidth / 1920.0f; // 横長に合わせる
            var w = rect.sizeDelta.x * _s;
            var h = rect.sizeDelta.y * _s;

            var texture = new Texture2D((int)w, (int)h);
            texture.ReadPixels(new Rect(corners[0].x, corners[0].y, w, h), 0, 0);
            texture.Apply();
            return texture;
        }
    }
}
