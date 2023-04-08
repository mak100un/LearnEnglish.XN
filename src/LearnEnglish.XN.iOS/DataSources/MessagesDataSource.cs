using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CoreAnimation;
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

    private async void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        switch (args.Action)
        {
            case NotifyCollectionChangedAction.Add when args.NewItems?.Count == 1:
                {
                    await AddAsync(args);

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
                    await AddAsync(args);
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
                await RemoveAsync(args);
                break;
            case NotifyCollectionChangedAction.Replace:
                await ReplaceAsync(args);
                break;
            case NotifyCollectionChangedAction.Move:
                await MoveAsync(args);
                break;
            case NotifyCollectionChangedAction.Reset:
                await ReloadAsync();
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

    private Task AddAsync(NotifyCollectionChangedEventArgs args)
    {
		return UpdateAsync(async () =>
        {
            var bottomOffset = CollectionView.ContentSize.Height - CollectionView.ContentOffset.Y;

            CATransaction.Begin();
            CATransaction.DisableActions = true;

            await CollectionView.PerformBatchUpdatesAsync(() =>
            {
                var indexPaths = CreateIndexesFrom(args.NewStartingIndex, args.NewItems.Count);
                CollectionView.InsertItems(indexPaths);
            });
            CollectionView.ContentOffset = new CGPoint(0, CollectionView.ContentSize.Height - bottomOffset);
            CATransaction.Commit();
        });
	}

    private Task RemoveAsync(NotifyCollectionChangedEventArgs args)
	{
		var startIndex = args.OldStartingIndex;

		if (startIndex < 0)
		{
            return ReloadAsync();
		}

        return UpdateAsync(async () => CollectionView.DeleteItems(CreateIndexesFrom(startIndex, args.OldItems.Count)));
	}

	private Task ReplaceAsync(NotifyCollectionChangedEventArgs args)
	{
		var newCount = args.NewItems.Count;

		if (newCount == args.OldItems.Count)
		{
            return UpdateAsync(async () => CollectionView.ReloadItems(CreateIndexesFrom(args.NewStartingIndex, newCount)));
        }

        return ReloadAsync();
    }

    private Task MoveAsync(NotifyCollectionChangedEventArgs args)
	{
		var count = args.NewItems.Count;

		if (count == 1)
		{
			var oldPath = NSIndexPath.Create(0, args.OldStartingIndex);
			var newPath = NSIndexPath.Create(0, args.NewStartingIndex);

            return UpdateAsync(async() => CollectionView.MoveItem(oldPath, newPath));
		}

		var start = Math.Min(args.OldStartingIndex, args.NewStartingIndex);
		var end = Math.Max(args.OldStartingIndex, args.NewStartingIndex) + count;

        return UpdateAsync(async() => CollectionView.ReloadItems(CreateIndexesFrom(start, end)));
	}

    private Task ReloadAsync()
    {
        CollectionView.ReloadData();
        CollectionView.CollectionViewLayout.InvalidateLayout();
        return Task.CompletedTask;
    }

    private async Task UpdateAsync(Func<Task> updateAsync)
    {
        if (CollectionView?.Hidden != false)
        {
            return;
        }

        await updateAsync();
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
