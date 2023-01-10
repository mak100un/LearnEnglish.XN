using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using Foundation;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Cells;
using PropertyChanged;
using Swordfish.NET.Collections;
using UIKit;

namespace LearnEnglish.XN.iOS.DataSources;

public class MessagesDataSource : UICollectionViewDataSource, INotifyPropertyChanged
{
    private readonly IReadOnlyDictionary<MessageTypes, string> _messageTypesToIdentifierMapper
        = Enum.GetValues(typeof(MessageTypes))
            .Cast<MessageTypes>()
            .ToDictionary(type => type, type => type.ToString());

    public MessagesDataSource(UICollectionView collectionView)
    {
        CollectionView = collectionView;
    }

    public UICollectionView CollectionView { get; }

    [OnChangedMethod(nameof(OnMessagesChanged))]
    public ConcurrentObservableCollection<MessageViewModel> Messages { get; set; }

    public event PropertyChangedEventHandler PropertyChanged;

    public override nint GetItemsCount(UICollectionView collectionView, nint section) => ItemCount;

    public override nint NumberOfSections(UICollectionView collectionView) => 1;

    protected int ItemCount => Messages?.Count ?? 0;

    public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
    {
        var message = Messages?.ElementAtOrDefault(indexPath.Row);

        if (message == null)
        {
            return null;
        }

        var cell = collectionView.DequeueReusableCell(_messageTypesToIdentifierMapper[message.MessageType], indexPath);

        return cell switch
        {
            BaseMessageCell messageCell => GetMessageCell(messageCell),
            _ => cell as UICollectionViewCell,
        };

        BaseMessageCell GetMessageCell(BaseMessageCell messageCell)
        {
            messageCell.DataContext = message;
            return messageCell;
        }
    }

    private void OnMessagesChanged(object oldValue, object newValue)
    {
        (oldValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        (newValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged += OnCollectionChanged);
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        switch (e.Action)
        {
            case NotifyCollectionChangedAction.Add when e.NewItems?.Count == 1:
                {
                    if (e.NewStartingIndex != 0 && LastVisibleItem() >= ItemCount - 3)
                    {
                       CollectionView.ScrollToItem(NSIndexPath.FromIndex((uint)((Messages?.Count - 1) ?? 0)), UICollectionViewScrollPosition.Bottom, true);
                    }

                    break;
                }
            case NotifyCollectionChangedAction.Add:
                {
                    if (LastVisibleItem() >= ItemCount - 3)
                    {
                        CollectionView.ScrollToItem(NSIndexPath.FromIndex((uint)((Messages?.Count - 1) ?? 0)), UICollectionViewScrollPosition.Bottom, true);
                    }
                    break;
                }

                int? LastVisibleItem() => CollectionView?.IndexPathsForVisibleItems.OrderBy(index => index.Row).LastOrDefault()?.Row;
        }
    }

    protected override void Dispose(bool disposing)
    {
        Messages?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        base.Dispose(disposing);
    }
}
