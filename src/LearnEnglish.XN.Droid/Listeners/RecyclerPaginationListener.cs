using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using AndroidX.RecyclerView.Widget;
using MvvmCross.Binding.BindingContext;

namespace LearnEnglish.XN.Droid.Listeners;

public class RecyclerPaginationListener : RecyclerView.OnScrollListener, IMvxBindingContextOwner, INotifyPropertyChanged
{
    private readonly LinearLayoutManager _layoutManager;

    public RecyclerPaginationListener(LinearLayoutManager layoutManager)
    {
        _layoutManager = layoutManager;
    }

    public ICommand LoadMoreCommand { get; set; }

    public int LoadingOffset { get; set; }

    public override void OnScrolled(RecyclerView recyclerView, int dx, int dy)
    {
        base.OnScrolled(recyclerView, dx, dy);

        if (_layoutManager.FindLastVisibleItemPosition() > _layoutManager.ItemCount - 1 - LoadingOffset)
        {
            return;
        }

        LoadMoreCommand?.Execute(null);
    }

    public IMvxBindingContext BindingContext { get; set; }
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(propertyName);
        return true;
    }
}
