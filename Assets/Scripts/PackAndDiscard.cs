using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackAndDiscard
{
    static List<Image> pack = new List<Image>(Resources.LoadAll<Image>("Images/Pack"));
    static List<Image> discard = new List<Image>();

    public static Image GetCard()
    {
        if (pack.Count == 0)
            RemixPack();
        Image sprite = pack[Random.Range(0, pack.Count)];
        pack.Remove(sprite);
        discard.Add(sprite);
        return sprite;
    }

    static void RemixPack()
    {
        pack.AddRange(discard);
        discard.Clear();
    }
}
