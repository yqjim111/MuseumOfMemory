using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Windows;

public class FlatDisplayController : ImageDisplayController
{
    public int imgCountPerRow;

    public float imgSize;

    GameObject selectedImage;

    float maxSelectDist = 50f;

    float dist;

    bool prevPauseState;

    override public void Init()
    {
        prevPauseState = false;
        dist = 0;
        paused = false;
        activeImages = new HashSet<GameObject>();
        objectPooler.Initialize();
        string albumPath = VRAConstants.rootPath + "\\" + albumName;
        DirectoryInfo d = new DirectoryInfo(albumPath);
        int count = 0;
        int row = 0;
        foreach (var file in d.EnumerateFiles("*.*", SearchOption.AllDirectories))
        {
            if (!Regex.IsMatch(file.Name, "\\.(?i)(jpe?g|png|gif|bmp)$")) continue;
            if (count >= imgCountPerRow)
            {
                row++;
                count = 0;
            }
            GameObject obj = objectPooler.SpawnFromPool("Image", spawnPoint.position, Quaternion.identity);
            obj.GetComponent<ImageObject>().SetObjectPoolRoot(objectPooler);
            Sprite texture = IMG2Sprite.LoadNewSprite(file.FullName);
            obj.GetComponent<ImageObject>().SetTexture(texture, imgSize);
            if (imgCountPerRow % 2 == 1)
                obj.transform.localPosition += new Vector3(imgCountPerRow > 1 ? (count - Mathf.FloorToInt(imgCountPerRow / 2)) * (20.0f / (imgCountPerRow - 1)) : 0, 0, obj.transform.localPosition.z + 5.0f * row);
            else
                obj.transform.localPosition += new Vector3(imgCountPerRow > 1 ? (count - imgCountPerRow / 2 + 0.5f) * (20.0f / (imgCountPerRow - 1)) : 0, 0, obj.transform.localPosition.z + 5.0f * row);
            activeImages.Add(obj);
            count++;
        }
    }

    public override void UpdateImages()
    {
        if (paused) return;
        foreach (var imageObj in activeImages)
        {
            imageObj.transform.position += flowDirection.normalized * speed * Time.deltaTime;
        }
    }

    public override void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            paused = !paused;
        }

        // Left click to select image
        if (Input.GetMouseButtonDown(0))
        {
            //create a ray cast and set it to the mouses cursor position in game
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            int layerMask = 1 << 2;
            layerMask = ~layerMask;
            if (Physics.Raycast(ray, out hit, maxSelectDist, layerMask))
            {
                prevPauseState = paused;
                paused = true;
                //draw invisible ray cast/vector
                Debug.DrawLine(ray.origin, hit.point);
                selectedImage = hit.collider.gameObject;
                Debug.Log(selectedImage.name);
                dist = hit.distance;
            }
        }
        // Drag to move image
        else if (Input.GetMouseButton(0) && selectedImage != null)
        {
            selectedImage.transform.position = Camera.main.transform.position + Camera.main.ScreenPointToRay(Input.mousePosition).direction.normalized * dist;
        } 
        // up to release image
        else if (Input.GetMouseButtonUp(0) && selectedImage != null)
        {
            dist = 0;
            selectedImage = null;
            paused = prevPauseState;
        }
        if (selectedImage != null && Input.mouseScrollDelta.y != 0)
        {
            if (Input.mouseScrollDelta.y > 0) dist += 1;
            else dist -= 1;
        }
    }
}
