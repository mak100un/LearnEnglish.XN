using System.Collections.Generic;

namespace LearnEnglish.XN.Core.Definitions.Extensions;

public static class ListExtensions
{
    public static void TryRemove<TItem>(this IList<TItem> list, TItem item)
    {
        if (list?.Contains(item) == true)
        {
            list.Remove(item);
        }
    }
}
