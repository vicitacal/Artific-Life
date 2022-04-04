using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapViewer : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    [SerializeField] private Map _map;

    void Start()
    {
        _texture = new Texture2D(MapCreator.MapSixeX, MapCreator.MapSixeY);
        gameObject.GetComponent<Renderer>().material.mainTexture = _texture;

        for (int i = 0; i < MapCreator.MapSixeX; i++)
            for (int j = 0; j < MapCreator.MapSixeY; j++)
            {
                float illumination = _map.Illumination[i, j] / 50f;
                _texture.SetPixel(i, j, new Color(illumination, illumination, illumination));
            }
        _texture.Apply();
    }
}
