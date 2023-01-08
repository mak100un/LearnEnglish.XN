using System;
using System.Collections.Generic;
using Foundation;
using LearnEnglish.XN.Core.ViewModels;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Platforms.Ios.Binding.Views;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;
using UIKit;

namespace LearnEnglish.XN.iOS.ViewControllers;

[MvxChildPresentation]
public class ChatViewController : MvxTableViewController<ChatViewModel>
{
    public override void ViewDidLoad()
    {
        base.ViewDidLoad();

        var source = new TableSource(TableView);
        this.AddBindings(new Dictionary<object, string>
        {
            {source, "ItemsSource Messages"}
        });

        TableView.Source = source;
        //TableView.RowHeight = KittenCell.GetCellHeight();
        TableView.ReloadData();
    }

    public class TableSource : MvxTableViewSource
    {
        private static readonly NSString KittenCellIdentifier = new NSString("KittenCell");
        private static readonly NSString DogCellIdentifier = new NSString("DogCell");

        public TableSource(UITableView tableView)
            : base(tableView)
        {
            tableView.SeparatorStyle = UITableViewCellSeparatorStyle.None;
            tableView.RegisterNibForCellReuse(UINib.FromName("KittenCell", NSBundle.MainBundle),
                KittenCellIdentifier);
            tableView.RegisterNibForCellReuse(UINib.FromName("DogCell", NSBundle.MainBundle), DogCellIdentifier);
        }

        protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath,
            object item)
        {
            NSString cellIdentifier;
            //if (item is Kitten)
            {
                cellIdentifier = KittenCellIdentifier;
            }
            //else if (item is Dog)
            {
                cellIdentifier = DogCellIdentifier;
            }
            //else
            {
                throw new ArgumentException("Unknown animal of type " + item.GetType().Name);
            }

            return (UITableViewCell) TableView.DequeueReusableCell(cellIdentifier, indexPath);
        }
    }
}
