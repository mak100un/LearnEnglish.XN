using System.Collections.Generic;
using MvvmCross.ViewModels;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels.Items;

public class MessageViewModel : MvxNotifyPropertyChanged
{
    public string Text { get; set; }

    [DoNotNotify]
    public bool IsMine { get; set; }

    public IEnumerable<VariantViewModel> Variants { get; set; }
}
