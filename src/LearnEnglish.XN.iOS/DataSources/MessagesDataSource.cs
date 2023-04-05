using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using CoreFoundation;
using CoreGraphics;
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

    private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add when args.NewItems?.Count == 1:
                {
                    Add(args);

                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        if (args.NewStartingIndex != 0 && LastVisibleItem() >= ItemCount - 3)
                        {
                            CollectionView.ScrollToItem(NSIndexPath.FromRowSection((Messages?.Count - 1) ?? 0, 0), UICollectionViewScrollPosition.Bottom, true);
                        }
                    });

                    break;
                }
            case NotifyCollectionChangedAction.Add:
                {
                    Add(args);
                    DispatchQueue.MainQueue.DispatchAsync(() =>
                    {
                        if (LastVisibleItem() >= ItemCount - 3)
                        {
                            CollectionView.ScrollToItem(NSIndexPath.FromRowSection((Messages?.Count - 1) ?? 0, 0),
                                UICollectionViewScrollPosition.Bottom, true);
                        }
                    });


                    break;
                }
            case NotifyCollectionChangedAction.Remove:
                Remove(args);
                break;
            case NotifyCollectionChangedAction.Replace:
                Replace(args);
                break;
            case NotifyCollectionChangedAction.Move:
                Move(args);
                break;
            case NotifyCollectionChangedAction.Reset:
                Reload();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        int? LastVisibleItem() => CollectionView?.IndexPathsForVisibleItems.OrderBy(index => index.Row).LastOrDefault()?.Row;
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
}
