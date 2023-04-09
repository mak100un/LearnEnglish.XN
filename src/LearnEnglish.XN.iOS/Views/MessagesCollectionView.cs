using System;
using System.Collections.Generic;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.Views;

public class MessagesCollectionView : UICollectionView
{
    private readonly Dictionary<NSIndexPath, CGSize> _sizeDictionary = new();

    public event EventHandler<NSIndexPath> ItemSizeUpdated;

    public MessagesCollectionView(CGRect frame, UICollectionViewLayout layout)
        : base(frame, layout)
    {
    }

    public Queue<bool> ScrollRequestsQueue { get; } = new();

    public override void ScrollToItem(NSIndexPath indexPath, UICollectionViewScrollPosition scrollPosition, bool animated)
    {
        ScrollRequestsQueue.Enqueue(true);
        base.ScrollToItem(indexPath, scrollPosition, animated);
    }

    public CGSize? TryGetItemSize(NSIndexPath indexPath) => _sizeDictionary.TryGetValue(indexPath, out CGSize size) ? size : null;

    public void TrySetItemSize(NSIndexPath indexPath, CGSize size)
    {
        var containsKey = _sizeDictionary.ContainsKey(indexPath);
        _sizeDictionary[indexPath] = size;
        if (containsKey)
        {
            return;
        }

        ItemSizeUpdated?.Invoke(this, indexPath);
    }

    public void TryClearItemSizes() => _sizeDictionary.Clear();
}
