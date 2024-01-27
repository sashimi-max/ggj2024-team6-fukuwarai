using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FacePartsData", menuName = "ScriptableObjects/CreateFacePartsSet")]
public class FacePartsAsset : ScriptableObject
{
    public List<FaceParts> facePartsSet = new List<FaceParts>();
}

[System.Serializable]
public class FaceParts
{
    public string name = "人間セット";
    public Sprite wholeFaceSprite;
    public FacePartsData[] facePartsData;
}

[System.Serializable]
public class FacePartsData
{
    public string name;
    public Sprite sprite;
    public float drag = 1.0f;
}
