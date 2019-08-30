
using System;
using System.Collections.Generic;
using System.Linq;
using Doods.Framework.Std;
using Doods.Framework.Std.Extensions;

namespace Doods.Framework.Mobile.Std.Servicies
{
    //internal class TelemetryService2 : ITelemetryService
    //{
    //    private readonly IKeysOptions _keysOption;
    //    private readonly string _cloudRoleNameBase;
    //    private readonly string _cloudRoleIntanceBase;

    //    private TelemetryClient _client;

    //    private Guid _sessionId = Guid.NewGuid();

    //    public TelemetryService(IKeysOptions options)
    //    {
    //        _keysOption = options;
    //        var appInsightKey = options?.AppInsightKey;
    //        if (string.IsNullOrEmpty(appInsightKey)) return;

    //        IsActive = true;
    //        _cloudRoleNameBase = options.AppInsightCloudRoleName;
    //        _cloudRoleIntanceBase = options.AppInsightCloudRoleInstance;
    //    }

    //    protected string ApplicationName { get; set; }

    //    protected string ModuleName { get; set; }

    //    protected string UtilisateurId { get; set; }

    //    protected string StructureId { get; set; }

    //    protected string SocieteAgricoleId { get; set; }

    //    protected string CycleProductionId { get; set; }

    //    public bool IsActive { get; protected set; }

    //    public Guid SessionId
    //    {
    //        get => _sessionId;
    //        set
    //        {
    //            _sessionId = value;
    //            _client = null;
    //        }
    //    }

    //    public void SetContext(string applicationName, string utilisateurName, int utilisateurId, string structureName,
    //        int structureId, int societeAgricoleId, int cycleProductionId)
    //    {
    //        if (!IsActive) return;

    //        ApplicationName = applicationName;
    //        UtilisateurId = utilisateurId > 0 ? $"{utilisateurId}" : string.Empty;
    //        SocieteAgricoleId = societeAgricoleId > 0 ? $"{societeAgricoleId}" : string.Empty;
    //        StructureId = structureId > 0 ? $"{structureId}" : string.Empty;
    //        CycleProductionId = cycleProductionId > 0 ? $"{cycleProductionId}" : string.Empty;

    //        Reset();
    //    }

    //    public void SetContext(string moduleName)
    //    {
    //        ModuleName = moduleName;

    //        Reset();
    //    }

    //    private void Reset()
    //    {
    //        _client = null;
    //    }

    //    public void Error(Exception e, Guid? operationId)
    //    {
    //        TraceException(e, "Error", operationId);
    //    }

    //    public void Fatal(Exception e, Guid? operationId)
    //    {
    //        TraceException(e, "Fatal", operationId);
    //    }

    //    public void Warning(Exception e, Guid? operationId)
    //    {
    //        TraceException(e, "Warning", operationId);
    //    }

    //    public void Event(string message, Dictionary<string, string> properties = null,
    //        Dictionary<string, double> measures = null, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackEvent(message, properties, measures);
    //    }

    //    public void Page(string name, TimeSpan duration, Dictionary<string, string> properties = null,
    //        Dictionary<string, double> measures = null, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        var page = new PageViewTelemetry(name);
    //        page.Duration = duration;
    //        if (properties.IsNotEmpty()) page.Properties.AddRange(properties);

    //        if (measures.IsNotEmpty()) page.Metrics.AddRange(measures);

    //        client.TrackPageView(page);
    //    }

    //    public void Metric(string name, double value, Dictionary<string, string> properties = null, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackMetric(name, value, properties);
    //    }

    //    public void Dependency(string type, string target, string name, string message, DateTimeOffset start,
    //        TimeSpan duration,
    //        string resultcode, bool success, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackDependency(type, target, name, message, start, duration, resultcode, success);
    //    }

    //    public void Exception(Exception exception, Dictionary<string, string> properties = null,
    //        Dictionary<string, double> measures = null, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackException(exception, properties, measures);
    //    }

    //    public void Request(string name, DateTimeOffset start, TimeSpan duration, string responseCode, bool success, Guid? operationId = null)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackRequest(name, start, duration, responseCode, success);
    //    }

    //    private TelemetryClient GetClient(Guid? operationId)
    //    {
    //        if (_client != null && operationId != null && !Equals(_client.Context.Operation.Id, $"{operationId}"))
    //        {
    //            Reset();
    //        }

    //        if (_client == null)
    //        {
    //            _client = new TelemetryClient();
    //            _client.Context.Session.Id = SessionId.ToString();
    //            _client.InstrumentationKey = _keysOption.AppInsightKey;

    //            _client.Context.Cloud.RoleName = FormatCloudRoleName();
    //            _client.Context.Cloud.RoleInstance = FormatCloudRoleInstance();

    //            if (!string.IsNullOrEmpty(UtilisateurId))
    //            {
    //                _client.Context.User.AccountId = $"{UtilisateurId}";
    //                _client.Context.User.AuthenticatedUserId = $"{UtilisateurId}";
    //                _client.Context.Properties.Add("uti", UtilisateurId);
    //            }

    //            if (!string.IsNullOrEmpty(SocieteAgricoleId)) _client.Context.Properties.Add("sai", SocieteAgricoleId);

    //            if (!string.IsNullOrEmpty(CycleProductionId)) _client.Context.Properties.Add("cpi", CycleProductionId);

    //            if (!string.IsNullOrEmpty(StructureId)) _client.Context.Properties.Add("sti", StructureId);

    //            if (operationId != null)
    //            {
    //                _client.Context.Operation.Id = $"{operationId}";
    //            }
    //        }

    //        return _client;
    //    }

    //    private string FormatCloudRoleInstance()
    //    {
    //        var value = new[]
    //            {
    //                _cloudRoleIntanceBase,
    //                ModuleName
    //            }.Where(e => !string.IsNullOrEmpty(e))
    //            .Join(":");

    //        if (string.IsNullOrEmpty(value))
    //        {
    //            value = "NONE";
    //        }

    //        return value.ToUpperInvariant();
    //    }

    //    private string FormatCloudRoleName()
    //    {
    //        var value = new[]
    //            {
    //                _cloudRoleNameBase,
    //                string.IsNullOrEmpty(ApplicationName) ? GetDefaultCloudRoleName() : ApplicationName
    //            }.Where(e => !string.IsNullOrEmpty(e))
    //            .Join(":");

    //        if (string.IsNullOrEmpty(value))
    //        {
    //            value = "NONE";
    //        }

    //        return value.ToUpperInvariant();
    //    }

    //    private string GetDefaultCloudRoleName()
    //    {
    //        var appinsight = WiuzParameterManager.Default.Read(ParameterManagerBase.AppInsightKey);
    //        if (string.IsNullOrEmpty(appinsight))
    //        {
    //            appinsight = WiuzParameterManager.Default.Read(ParameterManagerBase.NomLogicielKey);
    //        }
    //        return string.IsNullOrEmpty(appinsight)
    //            ? string.IsNullOrEmpty(_cloudRoleNameBase)
    //                ? "WIUZ"
    //                : string.Empty
    //            : appinsight;
    //    }

    //    private void TraceException(Exception e, string niveau, Guid? operationId)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackException(e, new Dictionary<string, string>
    //        {
    //            {
    //                "Niveau", niveau
    //            }
    //        });
    //    }

    //    public void Dependency(string type, string target, string name, string data, DateTime start, TimeSpan duration,
    //        string code, bool success, Guid? operationId)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackDependency(type, target, name, data, start, duration, code, success);
    //    }

    //    public void Request(string name, DateTime start, TimeSpan duration, string code, bool success, Guid? operationId)
    //    {
    //        if (!IsActive)
    //            return;

    //        var client = GetClient(operationId);
    //        client.TrackRequest(name, start, duration, code, success);
    //    }

    //    #region IDisposable Support

    //    private bool _disposedValue;

    //    protected virtual void InternalDispose(bool disposing)
    //    {
    //        if (!_disposedValue)
    //        {
    //            if (disposing) _client = null;

    //            _disposedValue = true;
    //        }
    //    }

    //    ~TelemetryService2()
    //    {
    //        // Ne modifiez pas ce code. Placez le code de nettoyage dans InternalDispose(bool disposing) ci-dessus.
    //        InternalDispose(false);
    //    }

    //    void IDisposable.Dispose()
    //    {
    //        InternalDispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    #endregion
    //}
}