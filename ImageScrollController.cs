using System;
using TenMaker.Utility;
using UnityEngine;
using UnityEngine.UI;

public class ImageScrollController : MonoBehaviour
{
    
    private Image _image;
    private Material _materialInstance;

    [SerializeField] private float scrollSpeed;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _materialInstance = Instantiate(_image.material);
        _image.material = _materialInstance;
    }

    void Update()
    {
        var offset = Vector2.one * (Time.time * scrollSpeed);
        _materialInstance.SetTextureOffset(TBMUtility_Shader.MainTex, offset);
    }
}