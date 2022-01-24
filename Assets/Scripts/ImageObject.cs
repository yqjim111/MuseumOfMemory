using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageObject : MonoBehaviour
{
    public Image imagePlane;

    private ObjectPooler objectPooler;

    public void SetObjectPoolRoot(ObjectPooler objectPooler)
    {
        this.objectPooler = objectPooler;
    }

    public void SetTexture(Sprite sprite, float size)
    {
        imagePlane.sprite = sprite;
        float factor = Mathf.Sqrt(size / (sprite.rect.width * sprite.rect.height));
        imagePlane.rectTransform.sizeDelta = sprite.rect.size * factor;
    }

    private void OnCollisionEnter(Collision collision)
    {

    }
}
