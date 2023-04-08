using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using UIKit;

namespace LearnEnglish.XN.iOS.Views;

public class MessagesCollectionView : UICollectionView
{
    private TaskCompletionSource<bool> _scrollTask;

    public MessagesCollectionView(CGRect frame, UICollectionViewLayout layout)
        : base(frame, layout)
    {
    }

    public async Task ScrollToItemAsync(NSIndexPath indexPath, UICollectionViewScrollPosition scrollPosition, bool animated)
    {
        _scrollTask?.TrySetResult(true);
        _scrollTask = new TaskCompletionSource<bool>();
        ScrollEnabled = false;
        ScrollToItem(indexPath, scrollPosition, animated);
        await _scrollTask.Task;
        ScrollEnabled = true;
    }

    public void ScrollAnimationEnd() => _scrollTask?.TrySetResult(true);
}
