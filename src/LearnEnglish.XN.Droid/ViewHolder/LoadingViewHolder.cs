using System;
using Android.Runtime;
using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace LearnEnglish.XN.Droid.ViewHolder;

public class LoadingViewHolder : RecyclerView.ViewHolder
{
    public LoadingViewHolder(IntPtr javaReference, JniHandleOwnership transfer)
        : base(javaReference, transfer)
    {
    }

    public LoadingViewHolder(View itemView)
        : base(itemView)
    {
    }
}
