using System;
using Autofac.Core;

namespace ZenAutofac.Extensions
{
    public static class DisposableExtensions
    {
        public static T AddTo<T>(this T disposable, IDisposer disposer)
            where T : IDisposable
        {
            if (disposable == null)
                throw new ArgumentNullException(nameof(disposable));

            if (disposer is null)
                throw new ArgumentNullException(nameof(disposer));

            disposer.AddInstanceForDisposal(disposable);

            return disposable;
        }
    }
}