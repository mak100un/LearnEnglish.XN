using System.Collections.Generic;
using System.Windows.Input;
using LearnEnglish.XN.Core.Definitions.Enums;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels.Items;

public class MessageViewModel : MvxNotifyPropertyChanged
{
    public string Text { get; set; }

    [DoNotNotify]
    public MessageTypes MessageType { get; set; } = MessageTypes.Mine;

    public IEnumerable<VariantViewModel> Variants { get; set; }

    [JsonIgnore]
    public ICommand SelectVariantCommand { get; set; }

    public bool IsMine => MessageType == MessageTypes.Mine;
}
