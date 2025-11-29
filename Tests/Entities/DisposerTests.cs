using System;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using ZenAutofac.Entities;

namespace Tests.Entities;

[TestFixture]
public class DisposerTests
{
    [Test]
    public void AddInstanceForDisposal_WhenInstanceIsNull_ThrowsArgumentNullException()
    {
        var disposer = new Disposer();

        Assert.Throws<ArgumentNullException>(() => disposer.AddInstanceForDisposal((IDisposable)null!));
    }

    [Test]
    public void AddInstanceForAsyncDisposal_WhenInstanceIsNull_ThrowsArgumentNullException()
    {
        var disposer = new Disposer();

        Assert.Throws<ArgumentNullException>(() => disposer.AddInstanceForAsyncDisposal((IAsyncDisposable)null!));
    }

    [Test]
    public void AddInstanceForDisposal_AfterDisposerDisposed_ThrowsObjectDisposedException()
    {
        var disposer = new Disposer();
        disposer.Dispose();

        Assert.Throws<ObjectDisposedException>(() => disposer.AddInstanceForDisposal(Substitute.For<IDisposable>()));
    }

    [Test]
    public void AddInstanceForAsyncDisposal_AfterDisposerDisposed_ThrowsObjectDisposedException()
    {
        var disposer = new Disposer();
        disposer.Dispose();

        Assert.Throws<ObjectDisposedException>(() => disposer.AddInstanceForAsyncDisposal(Substitute.For<IAsyncDisposable>()));
    }

    [Test]
    public void Dispose_DisposesAllSynchronousItems()
    {
        var disposer = new Disposer();
        var disposable1 = Substitute.For<IDisposable>();
        var disposable2 = Substitute.For<IDisposable>();

        disposer.AddInstanceForDisposal(disposable1);
        disposer.AddInstanceForDisposal(disposable2);

        disposer.Dispose();

        disposable1.Received(1).Dispose();
        disposable2.Received(1).Dispose();
    }

    [Test]
    public void Dispose_WhenOnlyAsyncItems_InvokesAsyncDisposeViaSyncPath()
    {
        var disposer = new Disposer();
        var asyncDisposable = new TestAsyncDisposable();

        disposer.AddInstanceForAsyncDisposal(asyncDisposable);

        disposer.Dispose();

        Assert.That(asyncDisposable.DisposeAsyncCallCount, Is.EqualTo(1));
    }

    [Test]
    public async Task DisposeAsync_DisposesAllAsyncItems()
    {
        var disposer = new Disposer();
        var asyncDisposable1 = new TestAsyncDisposable();
        var asyncDisposable2 = new TestAsyncDisposable();

        disposer.AddInstanceForAsyncDisposal(asyncDisposable1);
        disposer.AddInstanceForAsyncDisposal(asyncDisposable2);

        await disposer.DisposeAsync();

        Assert.That(asyncDisposable1.DisposeAsyncCallCount, Is.EqualTo(1));
        Assert.That(asyncDisposable2.DisposeAsyncCallCount, Is.EqualTo(1));
    }

    [Test]
    public async Task DisposeAsync_DisposesAsyncAndSynchronousItems()
    {
        var disposer = new Disposer();
        var asyncDisposable = new TestAsyncDisposable();
        var disposable = Substitute.For<IDisposable>();

        disposer.AddInstanceForAsyncDisposal(asyncDisposable);
        disposer.AddInstanceForDisposal(disposable);

        await disposer.DisposeAsync();

        Assert.That(asyncDisposable.DisposeAsyncCallCount, Is.EqualTo(1));
        disposable.Received(1).Dispose();
    }

    private sealed class TestAsyncDisposable : IAsyncDisposable
    {
        public int DisposeAsyncCallCount { get; private set; }

        public ValueTask DisposeAsync()
        {
            DisposeAsyncCallCount++;
            return ValueTask.CompletedTask;
        }
    }
}
