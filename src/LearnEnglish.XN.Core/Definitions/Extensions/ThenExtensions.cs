using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnEnglish.XN.Core.Definitions.Extensions;

public static class ThenExtensions
{
    public static TOut ThenIfNotEmpty<TIn, TOut>(this IEnumerable<TIn> @in, Func<IEnumerable<TIn>, TOut> action)
        => @in?.Any() == true ? action(@in) : default;

    public static void Then<TIn>(this TIn @in, Action<TIn> action)
        => action(@in);

    public static TOut Then<TIn, TOut>(this TIn @in, Func<TIn, TOut> action)
        => action(@in);

    public static async Task<TOut> ThenAsync<TIn, TOut>(this Task<TIn> inTask, Func<TIn, TOut> action)
    {
        var @in = await inTask;
        return action(@in);
    }

    public static async Task ThenAsync<TIn>(this TIn @in, Func<TIn, Task> action)
        => await action(@in);

    public static async Task<TOut> ThenAsync<TIn, TOut>(this TIn @in, Func<TIn, Task<TOut>> action)
        => await action(@in);

    public static async Task ThenAsync<TIn>(this Task<TIn> inTask, Action<TIn> action)
    {
        var @in = await inTask;
        action(@in);
    }

    public static async Task<TOut> ThenAsync<TOut>(this Task inTask, Func<TOut> action)
    {
        await inTask;
        return action();
    }
}
