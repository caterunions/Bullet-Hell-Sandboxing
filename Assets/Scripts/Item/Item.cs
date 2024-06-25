using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Generic Item", menuName = "Item/Generic Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    protected string _internalID;
    public string InternalID => _internalID;

    [SerializeField]
    protected string _displayName;
    public string DisplayName => _displayName;

    [SerializeField, TextArea(15,20)]
    protected string _description;
    public string Description => _description;

    [SerializeField]
    protected Sprite _sprite;
    public Sprite Sprite => _sprite;
}

//if only this worked
[CustomEditor(typeof(Item))]
public class ItemEditor : Editor
{
    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        Item item = (Item)target;

        if(item == null || item.Sprite == null)
        {
            return null;
        }

        Texture2D texture = new Texture2D(width, height);
        EditorUtility.CopySerialized(item.Sprite.texture, texture);
        return texture;
    }
}