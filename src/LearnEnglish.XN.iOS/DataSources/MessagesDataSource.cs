using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CoreAnimation;
using CoreFoundation;
using CoreGraphics;
using Foundation;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.Definitions.Extensions;
using LearnEnglish.XN.Core.ViewModels.Items;
using LearnEnglish.XN.iOS.Cells;
using LearnEnglish.XN.iOS.Views;
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

    public MessagesDataSource(MessagesCollectionView collectionView)
    {
        CollectionView = collectionView;
    }

    public MessagesCollectionView CollectionView { get; }

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
            LoaderCell loaderCell => GetLoaderCell(loaderCell),
            _ => cell as UICollectionViewCell,
        };

        BaseMessageCell GetMessageCell(BaseMessageCell messageCell)
        {
            messageCell.DataContext = message;
            return messageCell;
        }

        static LoaderCell GetLoaderCell(LoaderCell loaderCell)
        {
            loaderCell.StartAnimating();
            return loaderCell;
        }

    }

    private void OnMessagesChanged(object oldValue, object newValue)
    {
        (oldValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        (newValue as ConcurrentObservableCollection<MessageViewModel>)?.Then(value => value.CollectionChanged += OnCollectionChanged);
    }

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add:
                {
                    switch (ItemCount - args.NewItems?.Count)
                    {
                        case 0:
                            {
                                CollectionView.TryClearItemSizes();
                                CollectionView.ItemSizeUpdated -= OnItemSizeUpdated;
                                CollectionView.ItemSizeUpdated += OnItemSizeUpdated;
                                break;
                            }
                        case > 0 when args.NewStartingIndex == 0 && args.NewItems?.Count == 1:
                            {
                                WrapWithScrollDisabling(() => Add(args));

                                return;
                            }
                        // Pagination
                        case > 0 when args.NewStartingIndex == 0:
                            {
                                if (CollectionView.ContentSize.Height > CollectionView.Bounds.Height)
                                {
                                    CollectionView.ScrollRequestsQueue.Enqueue(true);
                                }

                                var bottomOffset = CollectionView.ContentSize.Height - CollectionView.ContentOffset.Y;

                                CATransaction.Begin();
                                CATransaction.DisableActions = true;

                                Add(args);

                                DispatchQueue.MainQueue.DispatchAsync(() =>
                                {
                                    if (CollectionView.ContentSize.Height <= CollectionView.Bounds.Height)
                                    {
                                        CATransaction.Commit();
                                        return;
                                    }

                                    CollectionView.ContentOffset = new CGPoint(0, CollectionView.ContentSize.Height - bottomOffset);
                                    CATransaction.Commit();
                                });

                                return;
                            }
                    }
                    Add(args);
                    DispatchQueue.MainQueue.DispatchAsync(ScrollToBottomAsync);

                    break;
                }
            case NotifyCollectionChangedAction.Remove:
                WrapWithScrollDisabling(() => Remove(args));
                break;
            case NotifyCollectionChangedAction.Replace:
                WrapWithScrollDisabling(() => Replace(args));
                break;
            case NotifyCollectionChangedAction.Move:
                WrapWithScrollDisabling(() => Move(args));
                break;
            case NotifyCollectionChangedAction.Reset:
                WrapWithScrollDisabling(Reload);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        void WrapWithScrollDisabling(Action action) => action();
    }

    private void OnItemSizeUpdated(object sender, NSIndexPath indexPath)
    {
        var lastItem = CollectionView.NumberOfItemsInSection(0) - 1;

        if (lastItem <  0)
        {
            return;
        }

        if (!indexPath.Equals(NSIndexPath.FromRowSection(lastItem, 0)))
        {
            return;
        }

        DispatchQueue.MainQueue.DispatchAsync(ScrollToBottomAsync);
    }

    protected override void Dispose(bool disposing)
    {
        Messages?.Then(value => value.CollectionChanged -= OnCollectionChanged);
        base.Dispose(disposing);
    }

    private void Add(NotifyCollectionChangedEventArgs args)
	{
		Update(() => CollectionView.InsertItems(CreateIndexesFrom(args.NewStartingIndex, args.NewItems.Count)), args);
	}

    private void Remove(NotifyCollectionChangedEventArgs args)
	{
		var startIndex = args.OldStartingIndex;

		if (startIndex < 0)
		{
			Reload();
			return;
		}

        Update(() => CollectionView.DeleteItems(CreateIndexesFrom(startIndex, args.OldItems.Count)), args);
	}

	private void Replace(NotifyCollectionChangedEventArgs args)
	{
		var newCount = args.NewItems.Count;

		if (newCount == args.OldItems.Count)
		{
			Update(() => CollectionView.ReloadItems(CreateIndexesFrom(args.NewStartingIndex, newCount)), args);
			return;
		}

		Reload();
	}

    private void Move(NotifyCollectionChangedEventArgs args)
	{
		var count = args.NewItems.Count;

		if (count == 1)
		{
			var oldPath = NSIndexPath.Create(0, args.OldStartingIndex);
			var newPath = NSIndexPath.Create(0, args.NewStartingIndex);

			Update(() => CollectionView.MoveItem(oldPath, newPath), args);
			return;
		}

		var start = Math.Min(args.OldStartingIndex, args.NewStartingIndex);
		var end = Math.Max(args.OldStartingIndex, args.NewStartingIndex) + count;

		Update(() => CollectionView.ReloadItems(CreateIndexesFrom(start, end)), args);
	}

    private void Reload()
    {
        CollectionView.ReloadData();
        CollectionView.CollectionViewLayout.InvalidateLayout();
    }

    private void Update(Action update, NotifyCollectionChangedEventArgs args)
    {
        if (CollectionView?.Hidden != false)
        {
            return;
        }

        update();
    }

    protected virtual NSIndexPath[] CreateIndexesFrom(int startIndex, int count)
    {
        var result = new NSIndexPath[count];

        for (var n = 0; n < count; n++)
        {
            result[n] = NSIndexPath.Create(0, startIndex + n);
        }

        return result;
    }

    private async void ScrollToBottomAsync()
    {
        if (CollectionView.ContentSize.Height <= CollectionView.Bounds.Height
            || CollectionView.IndexPathsForVisibleItems?.Any() != true
            || !(LastVisibleItem() >= ItemCount - 3))
        {
            return;
        }


        var lastItem = CollectionView.NumberOfItemsInSection(0) - 1;

        if (lastItem <  0)
        {
            return;
        }

        CollectionView.ScrollToItem(NSIndexPath.FromRowSection(lastItem, 0), UICollectionViewScrollPosition.Bottom, true);

        int? LastVisibleItem() => CollectionView?.IndexPathsForVisibleItems?.Max(index => index.Row);
    }
}
