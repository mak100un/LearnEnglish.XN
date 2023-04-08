using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewLayouts;

public class MessagesFlowDelegateLayout : UICollectionViewDelegateFlowLayout, INotifyPropertyChanged
{
    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public override void Scrolled(UIScrollView scrollView)
    {
        if (scrollView is not UICollectionView collectionView
            || collectionView.IndexPathsForVisibleItems?.Any() != true
            || collectionView.IndexPathsForVisibleItems.OrderBy(index => index.Row).FirstOrDefault()?.Row > LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

