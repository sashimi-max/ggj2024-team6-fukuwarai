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

            var w = rect.sizeDelta.x;
            var h = rect.sizeDelta.y;
            var screenWidth = Screen.currentResolution.width;
            var screenHeight = Screen.currentResolution.height;
#if UNITY_EDITOR
            string[] res = UnityStats.screenRes.Split('x');
            screenWidth = int.Parse(res[0]);
            screenHeight = int.Parse(res[1]);
#endif
            var x = (int)(screenWidth / 2 - rect.sizeDelta.x / 2);
            var y = (int)(screenHeight / 2 - rect.sizeDelta.y / 2);

            var texture = new Texture2D((int)w, (int)h);
            texture.ReadPixels(new Rect(x, y, w, h), 0, 0);
            texture.Apply();
            return texture;
        }
    }
}
