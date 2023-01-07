using System.Windows.Input;
using MvvmCross.ViewModels;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels.Items;

public class LaunchActionViewModel : MvxNotifyPropertyChanged
{
    [DoNotNotify]
    public ICommand ActionCommand { get; set; }

    public string Text { get; set; }
}
