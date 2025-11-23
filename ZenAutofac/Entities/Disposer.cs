using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac.Core;
using Autofac.Util;

namespace ZenAutofac.Entities
{
    public class Disposer : Disposable, IDisposer
    {
        private readonly SemaphoreSlim _syncRoot = new SemaphoreSlim(1, 1);

        /// <summary>Contents all implement IDisposable or IAsyncDisposable.</summary>
        private Stack<object> _items = new Stack<object>();

        /// <summary>
        /// Adds an object to the disposer, where that object only implements IAsyncDisposable. When the disposer is
        /// disposed, so will the object be.
        /// This is not typically recommended, and you should implement IDisposable as well.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <remarks>
        /// If this Disposer is disposed of using a synchronous Dispose call, that call will throw an exception.
        /// </remarks>
        public void AddInstanceForAsyncDisposal(IAsyncDisposable instance)
        {
            AddInternal(instance);
        }

        /// <summary>
        /// Adds an object to the disposer. When the disposer is
        /// disposed, so will the object be.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void AddInstanceForDisposal(IDisposable instance)
        {
            AddInternal(instance);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _syncRoot.Wait();
                try
                {
                    while (_items.Count > 0)
                    {
                        var obj = _items.Pop();

                        if (obj is IDisposable disposable)
                            disposable.Dispose();
                        else
                        {
                            if (obj is IAsyncDisposable asyncDisposable)
                                Task.Run(async () =>
                                        await asyncDisposable.DisposeAsync().ConfigureAwait(false))
                                    .ConfigureAwait(false)
                                    .GetAwaiter().GetResult();
                        }
                    }

                    _items = null;
                }
                finally
                {
                    _syncRoot.Release();
                    _syncRoot.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        /// <inheritdoc />
        protected override async ValueTask DisposeAsync(bool disposing)
        {
            if (!disposing)
                return;
            await _syncRoot.WaitAsync().ConfigureAwait(false);
            try
            {
                while (_items.Count > 0)
                {
                    var obj = _items.Pop();
                    if (obj is IAsyncDisposable asyncDisposable)
                        await asyncDisposable.DisposeAsync().ConfigureAwait(false);
                    else if (obj is IDisposable disposable)
                        disposable.Dispose();
                }

                _items = null;
            }
            finally
            {
                _syncRoot.Release();
                _syncRoot.Dispose();
            }
        }

        private void AddInternal(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            if (IsDisposed)
                throw new ObjectDisposedException(nameof(Disposer));

            _syncRoot.Wait();
            try
            {
                _items.Push(instance);
            }
            finally
            {
                _syncRoot.Release();
            }
        }
    }
}