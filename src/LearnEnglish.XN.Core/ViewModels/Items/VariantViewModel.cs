using System.Windows.Input;
using LearnEnglish.XN.Core.Definitions.Enums;
using MvvmCross.ViewModels;
using Newtonsoft.Json;
using PropertyChanged;

namespace LearnEnglish.XN.Core.ViewModels.Items;

public class VariantViewModel : MvxNotifyPropertyChanged
{
    public string Text { get; set; }

    public VariantTypes VariantType { get; set; }

    [DoNotNotify]
    public bool IsCorrect { get; set; }

    public string CorrectText { get; set; }

    [JsonIgnore]
    public ICommand SelectCommand { get; set; }
}
