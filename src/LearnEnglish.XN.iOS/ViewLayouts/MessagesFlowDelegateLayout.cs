using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewLayouts;

public class MessagesFlowDelegateLayout : UICollectionViewDelegateFlowLayout, INotifyPropertyChanged
{
    public MessagesFlowDelegateLayout(UICollectionView collectionView) => CollectionView = collectionView;

    public UICollectionView CollectionView { get; }

    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public override void Scrolled(UIScrollView scrollView)
    {
        if (CollectionView?.IndexPathsForVisibleItems?.Any() != true
            || CollectionView?.IndexPathsForVisibleItems.OrderBy(index => index.Row).FirstOrDefault()?.Row > LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

