namespace MZikmund.Toolkit.WinUI.Infrastructure;

/// <summary>
/// Hook for one-time / per-version migration logic that runs at app startup.
/// </summary>
/// <remarks>
/// The toolkit ships only the interface — concrete implementations stay per-app
/// because the migrations themselves are app-specific. Apps typically register
/// a single <see cref="IAppUpdater"/> in DI and invoke <see cref="UpdateAsync"/>
/// from their startup pipeline before the main shell is shown.
/// </remarks>
public interface IAppUpdater
{
    /// <summary>
    /// Runs any pending migrations or upgrade tasks. Should be idempotent — calling
    /// this multiple times must not corrupt state.
    /// </summary>
    Task UpdateAsync();
}
