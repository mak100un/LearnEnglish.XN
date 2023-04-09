using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.iOS.Views;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewLayouts;

public class MessagesFlowDelegateLayout : UICollectionViewDelegateFlowLayout, INotifyPropertyChanged
{
    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public override void Scrolled(UIScrollView scrollView)
    {
        if (scrollView.ContentSize.Height <= scrollView.Bounds.Height
            || scrollView is not MessagesCollectionView collectionView
            || collectionView.ScrollRequestsQueue.TryDequeue(out _)
            || collectionView.IndexPathsForVisibleItems?.Any() != true
            || collectionView.IndexPathsForVisibleItems.Min(index => index.Row) > LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public override CGSize GetSizeForItem(UICollectionView collectionView, UICollectionViewLayout layout, NSIndexPath indexPath)
    {
        if (collectionView is MessagesCollectionView messagesCollectionView
            && messagesCollectionView.TryGetItemSize(indexPath) is { } size)
        {
            return size;
        }

        return new CGSize(1, 1);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

