using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using MvvmCross.Binding.BindingContext;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewLayouts;

public class MessagesFlowDelegateLayout : UICollectionViewDelegateFlowLayout, INotifyPropertyChanged
{
    public MessagesFlowDelegateLayout(UICollectionView collectionView)
    {
        CollectionView = collectionView;
    }

    public UICollectionView CollectionView { get; }

    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public override void Scrolled(UIScrollView scrollView)
    {
        base.Scrolled(scrollView);
        if (CollectionView?.IndexPathsForVisibleItems.OrderBy(index => index.Row).FirstOrDefault()?.Row > LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public event PropertyChangedEventHandler PropertyChanged;
}

