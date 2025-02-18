using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

//個別のアイテムのデータ
public class Item:MonoBehaviour
{
    [Tooltip("アイテムの画像")]
    public Sprite ItemImage;
    [Tooltip("アイテムの名前")]
    public string ItemName;
    public string Description;
}
