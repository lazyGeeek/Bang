using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackAndDiscard
{
    static List<Sprite> pack = new List<Sprite>(Resources.LoadAll<Sprite>("Images/Pack"));
    static List<Sprite> discard = new List<Sprite>();

    public static Sprite GetCard()
    {
        if (pack.Count == 0)
            RemixPack();
        Sprite sprite = pack[Random.Range(0, pack.Count)];
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
