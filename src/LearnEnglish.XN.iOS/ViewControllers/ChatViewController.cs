using System;
using System.Collections.Generic;
using Cirrious.FluentLayouts.Touch;
using CoreGraphics;
using LearnEnglish.XN.Core.Definitions.Enums;
using LearnEnglish.XN.Core.ViewModels;
using LearnEnglish.XN.iOS.Cells;
using LearnEnglish.XN.iOS.DataSources;
using LearnEnglish.XN.iOS.ViewLayouts;
using LearnEnglish.XN.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

[MvxChildPresentation]
public class ChatViewController : BaseViewController<ChatViewModel>
{
    private readonly IReadOnlyDictionary<string, Type> _messageTypesToCellsMapper = new Dictionary<string, Type>
    {
        [nameof(MessageTypes.Loading)] = typeof(LoaderCell),
        [nameof(MessageTypes.Mine)] = typeof(MyMessageCell),
        [nameof(MessageTypes.Operator)] = typeof(OperatorMessageCell),
        [nameof(MessageTypes.OperatorWithVariants)] = typeof(OperatorMessageWithVariantsCell),
    };

    private MessagesCollectionView _collectionView;
    private MessagesDataSource _dataSource;
    private MessagesFlowDelegateLayout _flowDelegateLayout;

    protected override void CreateView()
    {
        base.CreateView();

        using var config = new UICollectionLayoutListConfiguration(UICollectionLayoutListAppearance.Plain)
        {
            ShowsSeparators = false,
            BackgroundColor = UIColor.Clear,
        };
        using var layout = UICollectionViewCompositionalLayout.GetLayout(config);
        View.Add(_collectionView = new MessagesCollectionView(CGRect.Empty, layout)
        {
            ScrollsToTop = false,
        });
        _collectionView.AutoresizingMask = UIViewAutoresizing.All;
        _collectionView.BackgroundView = new UIImageView
        {
            Image = new UIImage("background_img.jpeg"),
            ContentMode = UIViewContentMode.ScaleAspectFill,
        };

        foreach (var messageCell in _messageTypesToCellsMapper)
        {
            _collectionView.RegisterClassForCell(messageCell.Value, messageCell.Key);
        }
        _collectionView.DataSource = (_dataSource = new MessagesDataSource(_collectionView));
        _collectionView.Delegate = (_flowDelegateLayout = new MessagesFlowDelegateLayout());
    }

    protected override void BindView()
    {
        base.BindView();

        var set = this.CreateBindingSet<ChatViewController, ChatViewModel>();

        set.Bind(_flowDelegateLayout)
            .For(x => x.LoadMoreCommand)
            .To(vm => vm.LoadMoreCommand);

        set.Bind(_flowDelegateLayout)
            .For(x => x.LoadingOffset)
            .To(vm => vm.LoadingOffset);

        set
            .Bind(_dataSource)
            .For(v => v.Messages)
            .To(vm => vm.Messages);

        set.Apply();
    }

    protected override void LayoutView()
    {
        base.LayoutView();
        var safeAreaGuide = View.SafeAreaLayoutGuide;
        NSLayoutConstraint.ActivateConstraints(new []
        {
            _collectionView.BottomAnchor.ConstraintEqualTo(safeAreaGuide.BottomAnchor),
            _collectionView.TopAnchor.ConstraintEqualTo(TopLayoutGuide.GetBottomAnchor()),
            _collectionView.LeadingAnchor.ConstraintEqualTo(safeAreaGuide.LeadingAnchor),
            _collectionView.TrailingAnchor.ConstraintEqualTo(safeAreaGuide.TrailingAnchor),
        });
    }
}
