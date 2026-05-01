using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MZikmund.Toolkit.WinUI.ComponentModel;
using MZikmund.Toolkit.WinUI.Infrastructure;
using MZikmund.Toolkit.WinUI.ViewModels;

namespace MZikmund.Toolkit.WinUI.Tests;

[TestClass]
public class ObservableObjectTests
{
    [TestMethod]
    public void SetProperty_ChangesValue_AndRaisesPropertyChanged()
    {
        var subject = new Subject();
        var captured = new List<string?>();
        subject.PropertyChanged += (_, e) => captured.Add(e.PropertyName);

        subject.Name = "Alice";

        Assert.AreEqual("Alice", subject.Name);
        CollectionAssert.AreEqual(new[] { "Name" }, captured);
    }

    [TestMethod]
    public void SetProperty_NoChange_DoesNotRaisePropertyChanged()
    {
        var subject = new Subject { Name = "Alice" };
        var captured = new List<string?>();
        subject.PropertyChanged += (_, e) => captured.Add(e.PropertyName);

        subject.Name = "Alice";

        Assert.AreEqual(0, captured.Count);
    }

    private sealed class Subject : ObservableObject
    {
        private string _name = string.Empty;

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
    }
}

[TestClass]
public class RelayCommandTests
{
    [TestMethod]
    public void Ctor_NullExecute_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => new RelayCommand(null!));
    }

    [TestMethod]
    public void CanExecute_NoPredicate_ReturnsTrue()
    {
        var command = new RelayCommand(() => { });

        Assert.IsTrue(command.CanExecute(null));
    }

    [TestMethod]
    public void CanExecute_WithPredicate_ReflectsPredicate()
    {
        var allow = false;
        var command = new RelayCommand(() => { }, () => allow);

        Assert.IsFalse(command.CanExecute(null));
        allow = true;
        Assert.IsTrue(command.CanExecute(null));
    }

    [TestMethod]
    public void Execute_WhenAllowed_RunsAction()
    {
        var ran = 0;
        var command = new RelayCommand(() => ran++);

        command.Execute(null);

        Assert.AreEqual(1, ran);
    }

    [TestMethod]
    public void Execute_WhenNotAllowed_SkipsAction()
    {
        var ran = 0;
        var command = new RelayCommand(() => ran++, () => false);

        command.Execute(null);

        Assert.AreEqual(0, ran);
    }

    [TestMethod]
    public void RaiseCanExecuteChanged_FiresEvent()
    {
        var command = new RelayCommand(() => { });
        var fired = 0;
        command.CanExecuteChanged += (_, _) => fired++;

        command.RaiseCanExecuteChanged();

        Assert.AreEqual(1, fired);
    }
}

[TestClass]
public class ViewModelBaseTests
{
    [TestMethod]
    public void IsLoading_RaisesPropertyChanged()
    {
        var vm = new TestViewModel();
        var captured = new List<string?>();
        vm.PropertyChanged += (_, e) => captured.Add(e.PropertyName);

        vm.IsLoading = true;

        CollectionAssert.AreEqual(new[] { "IsLoading" }, captured);
    }

    [TestMethod]
    public void DefaultLifecycleHooks_DoNotThrow()
    {
        var vm = new TestViewModel();

        vm.ViewCreated();
        vm.ViewLoading();
        vm.ViewLoaded();
        vm.ViewUnloaded();
        vm.OnNavigatedTo("param");
        vm.OnNavigatedFrom();

        // Reaching this line means none of the virtual base implementations threw.
        Assert.IsTrue(true);
    }

    private sealed class TestViewModel : ViewModelBase
    {
    }
}

[TestClass]
public class WindowShellViewModelTests
{
    [TestMethod]
    public void GoBackCommand_RaisesGoBackRequested()
    {
        var vm = new WindowShellViewModel();
        var fired = 0;
        vm.GoBackRequested += (_, _) => fired++;

        vm.GoBackCommand.Execute(null);

        Assert.AreEqual(1, fired);
    }

    [TestMethod]
    public void Title_RaisesPropertyChanged()
    {
        var vm = new WindowShellViewModel();
        var captured = new List<string?>();
        ((INotifyPropertyChanged)vm).PropertyChanged += (_, e) => captured.Add(e.PropertyName);

        vm.Title = "Test";

        CollectionAssert.AreEqual(new[] { "Title" }, captured);
    }

    [TestMethod]
    public void LoadingStatusMessage_RaisesPropertyChanged()
    {
        var vm = new WindowShellViewModel();
        var captured = new List<string?>();
        ((INotifyPropertyChanged)vm).PropertyChanged += (_, e) => captured.Add(e.PropertyName);

        vm.LoadingStatusMessage = "Saving…";

        CollectionAssert.AreEqual(new[] { "LoadingStatusMessage" }, captured);
    }
}

[TestClass]
public class IoCTests
{
    [TestCleanup]
    public void Cleanup() => IoC.Reset();

    [TestMethod]
    public void GetService_BeforeSetProvider_Throws()
    {
        IoC.Reset();

        Assert.Throws<InvalidOperationException>(() => IoC.GetService<ISampleService>());
    }

    [TestMethod]
    public void GetRequiredService_BeforeSetProvider_Throws()
    {
        IoC.Reset();

        Assert.Throws<InvalidOperationException>(() => IoC.GetRequiredService<ISampleService>());
    }

    [TestMethod]
    public void SetProvider_Null_Throws()
    {
        Assert.Throws<ArgumentNullException>(() => IoC.SetProvider(null!));
    }

    [TestMethod]
    public void GetService_RegisteredService_Returns()
    {
        var provider = new TinyProvider().Register<ISampleService>(new SampleService());
        IoC.SetProvider(provider);

        Assert.IsNotNull(IoC.GetService<ISampleService>());
    }

    [TestMethod]
    public void GetService_UnregisteredService_ReturnsNull()
    {
        IoC.SetProvider(new TinyProvider());

        Assert.IsNull(IoC.GetService<ISampleService>());
    }

    [TestMethod]
    public void GetRequiredService_UnregisteredService_Throws()
    {
        IoC.SetProvider(new TinyProvider());

        Assert.Throws<InvalidOperationException>(() => IoC.GetRequiredService<ISampleService>());
    }

    [TestMethod]
    public void Reset_ClearsRegisteredProvider()
    {
        IoC.SetProvider(new TinyProvider().Register<ISampleService>(new SampleService()));

        IoC.Reset();

        Assert.Throws<InvalidOperationException>(() => IoC.GetService<ISampleService>());
    }

    private interface ISampleService
    {
    }

    private sealed class SampleService : ISampleService
    {
    }

    private sealed class TinyProvider : IServiceProvider
    {
        private readonly Dictionary<Type, object> _services = new();

        public TinyProvider Register<T>(T instance) where T : class
        {
            _services[typeof(T)] = instance;
            return this;
        }

        public object? GetService(Type serviceType) =>
            _services.TryGetValue(serviceType, out var s) ? s : null;
    }
}
