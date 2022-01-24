using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

// a helper class that can dynamically load textures from path.
public class IMG2Sprite
{
    public static Sprite LoadNewSprite(string FilePath, float PixelsPerUnit = 1)
    {

        // Load a PNG or JPG image from disk to a Texture2D, assign this texture to a new sprite and return its reference

        Sprite NewSprite;
        Texture2D SpriteTexture = LoadTexture(FilePath);
        NewSprite = Sprite.Create(SpriteTexture, new Rect(0, 0, SpriteTexture.width, SpriteTexture.height), new Vector2(0.5f, 0.5f), PixelsPerUnit);
        return NewSprite;
    }

    public static Texture2D LoadTexture(string FilePath)
    {

        // Load a PNG or JPG file from disk to a Texture2D
        // Returns null if load fails

        Texture2D Tex2D;
        byte[] FileData;

        if (File.Exists(FilePath))
        {
            FileData = File.ReadAllBytes(FilePath);
            Tex2D = new Texture2D(2, 2);           // Create new "empty" texture
            if (Tex2D.LoadImage(FileData))           // Load the imagedata into the texture (size is set automatically)
                return Tex2D;                 // If data = readable -> return texture
        }
        return null;                     // Return null if load failed
    }
}

public class ImageDisplayController : MonoBehaviour
{

    public Vector3 flowDirection;

    public float speed;

    public GameObject imageRoot;

    public Transform spawnPoint;

    public GameObject boundingboxRoot;

    public ObjectPooler objectPooler;

    public string albumName;

    public HashSet<GameObject> activeImages;

    public bool paused;

    // Start is called before the first frame update
    public virtual void Start()
    {
        Init();
    }

    // Update is called once per frame
    public virtual void Update()
    {
        HandleInputs();
        UpdateImages();
    }

    public virtual void Init()
    {
        throw new System.Exception("Need to implement initialization function of controller");
    }

    public virtual void UpdateImages()
    {
        throw new System.Exception("Need to implement image update function of controller");
    }

    public virtual void HandleInputs()
    {
        throw new System.Exception("Need to implement the keyboard input scheme");
    }
}
