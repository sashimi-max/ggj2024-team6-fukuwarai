using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ.Title.WebImage
{
    public class GalleryFactory : MonoBehaviour
    {
        [SerializeField] WebImageDownloader webImageDownloader;
        [SerializeField] Transform gallerySpawnTransform;
        [SerializeField] Image galleryImagePrefab;

        // Start is called before the first frame update
        void Start()
        {
            webImageDownloader.OnDownLoadImage
                .Subscribe(blob => SpawnGallery(blob))
                .AddTo(this);

            webImageDownloader.DownloadImageRecentry().Forget();
        }

        private void SpawnGallery(byte[] blob)
        {
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(blob);

            var obj = Instantiate(galleryImagePrefab, gallerySpawnTransform);
            var resizeTexture = ResizeTexture(texture, (int)obj.rectTransform.sizeDelta.x, (int)obj.rectTransform.sizeDelta.y);
            obj.sprite = Sprite.Create(resizeTexture, new Rect(0, 0, resizeTexture.width, resizeTexture.height), Vector2.zero);
        }

        Texture2D ResizeTexture(Texture2D originalTexture, int newWidth, int newHeight)
        {
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            RenderTexture.active = rt;
            Graphics.Blit(originalTexture, rt);

            Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
            resizedTexture.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            resizedTexture.Apply();

            RenderTexture.active = null;
            RenderTexture.ReleaseTemporary(rt);

            return resizedTexture;
        }
    }
}