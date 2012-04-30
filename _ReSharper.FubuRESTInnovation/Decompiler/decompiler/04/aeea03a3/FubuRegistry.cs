// Type: FubuMVC.Core.FubuRegistry
// Assembly: FubuMVC.Core, Version=0.9.5.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\dev\git projectsx\FubuRESTInnovation\packages\FubuMVC.References.0.9.5.817\lib\net40\FubuMVC.Core.dll

using FubuCore;
using FubuCore.Formatting;
using FubuCore.Reflection;
using FubuCore.Util;
using FubuMVC.Core.Ajax;
using FubuMVC.Core.Assets;
using FubuMVC.Core.Behaviors;
using FubuMVC.Core.Diagnostics;
using FubuMVC.Core.Http;
using FubuMVC.Core.Http.Headers;
using FubuMVC.Core.Packaging;
using FubuMVC.Core.Registration;
using FubuMVC.Core.Registration.Conventions;
using FubuMVC.Core.Registration.DSL;
using FubuMVC.Core.Registration.Nodes;
using FubuMVC.Core.Registration.ObjectGraph;
using FubuMVC.Core.Resources;
using FubuMVC.Core.Resources.PathBased;
using FubuMVC.Core.Runtime;
using FubuMVC.Core.Security;
using FubuMVC.Core.UI;
using FubuMVC.Core.View.Activation;
using FubuMVC.Core.View.Attachment;
using HtmlTags;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;

namespace FubuMVC.Core
{
  public class FubuRegistry : IFubuRegistry
  {
    private readonly ActionMethodFilter _methodFilter = new ActionMethodFilter();
    private readonly IList<Type> _importedTypes = (IList<Type>) new List<Type>();
    private readonly List<IConfigurationAction> _conventions = new List<IConfigurationAction>();
    private readonly List<IConfigurationAction> _explicits = new List<IConfigurationAction>();
    private readonly List<FubuRegistry.RegistryImport> _imports = new List<FubuRegistry.RegistryImport>();
    private readonly IList<Action<FubuRegistry>> _importsConventions = (IList<Action<FubuRegistry>>) new List<Action<FubuRegistry>>();
    private readonly List<IConfigurationAction> _policies = new List<IConfigurationAction>();
    private readonly RouteDefinitionResolver _routeResolver = new RouteDefinitionResolver();
    private readonly IList<IServiceRegistry> _serviceRegistrations = (IList<IServiceRegistry>) new List<IServiceRegistry>();
    private readonly List<IConfigurationAction> _systemPolicies = new List<IConfigurationAction>();
    private readonly TypePool _types = new TypePool(FubuRegistry.FindTheCallingAssembly());
    private readonly List<IActionSource> _actionSources = new List<IActionSource>();
    private readonly IList<Action<TypePool>> _scanningOperations = (IList<Action<TypePool>>) new List<Action<TypePool>>();
    private DiagnosticLevel _diagnosticLevel;
    private bool _diagnosticsRegistryImported;
    private readonly ViewAttacherConvention _viewAttacherConvention;
    private readonly ViewBagConventionRunner _bagRunner;
    private IConfigurationObserver _observer;
    private readonly ConnegAttachmentPolicy _connegAttachmentPolicy;
    private readonly BehaviorAggregator _behaviorAggregator;

    public RouteConventionExpression Routes
    {
      get
      {
        return new RouteConventionExpression(this._routeResolver, this);
      }
    }

    public OutputDeterminationExpression Output
    {
      get
      {
        return new OutputDeterminationExpression(this);
      }
    }

    public ViewExpression Views
    {
      get
      {
        return new ViewExpression(this._bagRunner, this, this._viewAttacherConvention);
      }
    }

    public PoliciesExpression Policies
    {
      get
      {
        return new PoliciesExpression((IList<IConfigurationAction>) this._policies, this._systemPolicies);
      }
    }

    public ModelsExpression Models
    {
      get
      {
        return new ModelsExpression(new Action<Action<IServiceRegistry>>(this.Services));
      }
    }

    public AppliesToExpression Applies
    {
      get
      {
        return new AppliesToExpression(this._types);
      }
    }

    public ActionCallCandidateExpression Actions
    {
      get
      {
        return new ActionCallCandidateExpression((IList<IActionSource>) this._actionSources, this._methodFilter);
      }
    }

    public MediaExpression Media
    {
      get
      {
        return new MediaExpression(this, this._connegAttachmentPolicy);
      }
    }

    public DiagnosticLevel DiagnosticLevel
    {
      get
      {
        return this._diagnosticLevel;
      }
    }

    public AssetsExpression Assets
    {
      get
      {
        return new AssetsExpression(this);
      }
    }

    public virtual string Name
    {
      get
      {
        return this.GetType().ToString();
      }
    }

    public FubuRegistry()
    {
      this._behaviorAggregator = new BehaviorAggregator(this._types, (IEnumerable<IActionSource>) this._actionSources);
      this._observer = (IConfigurationObserver) new NulloConfigurationObserver();
      this._viewAttacherConvention = new ViewAttacherConvention();
      this._bagRunner = new ViewBagConventionRunner(this._types);
      this._connegAttachmentPolicy = new ConnegAttachmentPolicy(this._types);
      this.setupDefaultConventionsAndPolicies();
    }

    public FubuRegistry(Action<FubuRegistry> configure)
      : this()
    {
      configure(this);
    }

    public void ApplyHandlerConventions()
    {
      this.ApplyHandlerConventions(new Type[1]
      {
        this.GetType()
      });
    }

    public void ApplyHandlerConventions<T>() where T : class
    {
      this.ApplyHandlerConventions(new Type[1]
      {
        typeof (T)
      });
    }

    public void ApplyHandlerConventions(params Type[] markerTypes)
    {
      this.ApplyHandlerConventions((Func<Type[], HandlersUrlPolicy>) (markers => new HandlersUrlPolicy(markers)), markerTypes);
    }

    public void ApplyHandlerConventions(Func<Type[], HandlersUrlPolicy> policyBuilder, params Type[] markerTypes)
    {
      GenericEnumerableExtensions.Each<Type>((IEnumerable<Type>) markerTypes, (Action<Type>) (t => this.Applies.ToAssembly(t.Assembly)));
      FubuRegistry.HandlerActionSource handlerActionSource = new FubuRegistry.HandlerActionSource((IEnumerable<Type>) markerTypes);
      if (this._actionSources.Contains((IActionSource) handlerActionSource))
        return;
      this.Routes.UrlPolicy((IUrlPolicy) policyBuilder(markerTypes));
      this.Actions.FindWith((IActionSource) handlerActionSource);
    }

    private void setupDefaultConventionsAndPolicies()
    {
      this._bagRunner.Apply((IViewBagConvention) this._viewAttacherConvention);
      this.ApplyConvention<BehaviorAggregator>(this._behaviorAggregator);
      this.ApplyConvention<PartialOnlyConvention>();
      this.addConvention((Action<BehaviorGraph>) (graph => this._routeResolver.ApplyToAll(graph)));
      this._systemPolicies.Add((IConfigurationAction) new AttachAuthorizationPolicy());
      this.ApplyConvention<DictionaryOutputConvention>();
      ActionCallFilterExpression toHtml1 = this.Output.ToHtml;
      ParameterExpression parameterExpression1 = Expression.Parameter(typeof (ActionCall), "x");
      // ISSUE: method reference
      // ISSUE: method reference
      Expression<Func<ActionCall, bool>> func1 = Expression.Lambda<Func<ActionCall, bool>>((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ReflectionExtensions.HasAttribute)), new Expression[1]
      {
        (Expression) Expression.Property((Expression) parameterExpression1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ActionCallBase.get_Method)))
      }), new ParameterExpression[1]
      {
        parameterExpression1
      });
      toHtml1.WhenCallMatches(func1);
      ActionCallFilterExpression toHtml2 = this.Output.ToHtml;
      ParameterExpression parameterExpression2 = Expression.Parameter(typeof (ActionCall), "x");
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      // ISSUE: method reference
      Expression<Func<ActionCall, bool>> func2 = Expression.Lambda<Func<ActionCall, bool>>((Expression) Expression.AndAlso((Expression) Expression.Call((Expression) Expression.Call((Expression) Expression.Property((Expression) Expression.Property((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ActionCallBase.get_Method))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (MemberInfo.get_Name))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.ToLower)), new Expression[0]), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.EndsWith)), new Expression[1]
      {
        (Expression) Expression.Constant((object) "html", typeof (string))
      }), (Expression) Expression.Equal((Expression) Expression.Call((Expression) parameterExpression2, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (ActionCallBase.OutputType)), new Expression[0]), (Expression) Expression.Constant((object) typeof (string), typeof (Type)), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Type.op_Equality)))), new ParameterExpression[1]
      {
        parameterExpression2
      });
      toHtml2.WhenCallMatches(func2);
      this.Output.ToJson.WhenTheOutputModelIs<JsonMessage>();
      this.Output.To<RenderHtmlDocumentNode>().WhenTheOutputModelIs<HtmlDocument>();
      this.Output.To<RenderHtmlTagNode>().WhenTheOutputModelIs<HtmlTag>();
      this.Output.ToBehavior<RenderStatusCodeBehavior>().WhenTheOutputModelIs<HttpStatusCode>();
      this.Policies.Add<AjaxContinuationPolicy>();
      this.Policies.Add<ContinuationHandlerConvention>();
      this.Policies.Add<AsyncContinueWithHandlerConvention>();
      this.Policies.Add<HeaderWritingPolicy>();
      this.Policies.Add<ResourcePathRoutePolicy>();
      this._systemPolicies.Add((IConfigurationAction) new StringOutputPolicy());
      this._systemPolicies.Add((IConfigurationAction) new MissingRouteInputPolicy());
      this.Models.BindPropertiesWith<CurrentRequestFullUrlPropertyBinder>();
      this.Models.BindPropertiesWith<CurrentRequestRelativeUrlPropertyBinder>();
      this._conventions.Add((IConfigurationAction) this._bagRunner);
      this.Policies.Add<JsonMessageInputConvention>();
      this._systemPolicies.Add((IConfigurationAction) this._connegAttachmentPolicy);
      this.ApplyConvention<ModifyChainAttributeConvention>();
    }

    public void HtmlConvention<T>() where T : HtmlConventionRegistry, new()
    {
      this.HtmlConvention((HtmlConventionRegistry) Activator.CreateInstance<T>());
    }

    public void HtmlConvention(HtmlConventionRegistry conventions)
    {
      this.Services((Action<IServiceRegistry>) (x => x.AddService<HtmlConventionRegistry>(conventions)));
    }

    public void HtmlConvention(Action<HtmlConventionRegistry> configure)
    {
      HtmlConventionRegistry conventions = new HtmlConventionRegistry();
      configure(conventions);
      this.HtmlConvention(conventions);
    }

    public void StringConversions<T>() where T : DisplayConversionRegistry, new()
    {
      this.addStringConversions((DisplayConversionRegistry) Activator.CreateInstance<T>());
    }

    private void addStringConversions(DisplayConversionRegistry conversions)
    {
      this.Services((Action<IServiceRegistry>) (x => x.AddService(typeof (DisplayConversionRegistry), ObjectDef.ForValue((object) conversions))));
    }

    public void StringConversions(Action<DisplayConversionRegistry> configure)
    {
      DisplayConversionRegistry conversions = new DisplayConversionRegistry();
      configure(conversions);
      this.addStringConversions(conversions);
    }

    public void UsingObserver(IConfigurationObserver observer)
    {
      this._observer = observer;
    }

    public void Services(Action<IServiceRegistry> configure)
    {
      ServiceRegistry serviceRegistry = new ServiceRegistry();
      configure((IServiceRegistry) serviceRegistry);
      this._serviceRegistrations.Add((IServiceRegistry) serviceRegistry);
    }

    public void Services<T>() where T : IServiceRegistry, new()
    {
      this._serviceRegistrations.Add((IServiceRegistry) new T());
    }

    public void ApplyConvention<TConvention>() where TConvention : IConfigurationAction, new()
    {
      this.ApplyConvention<TConvention>(new TConvention());
    }

    public void ApplyConvention<TConvention>(TConvention convention) where TConvention : IConfigurationAction
    {
      this._conventions.Add((IConfigurationAction) convention);
    }

    public ChainedBehaviorExpression Route(string pattern)
    {
      ExplicitRouteConfiguration routeConfiguration = new ExplicitRouteConfiguration(pattern);
      this._explicits.Add((IConfigurationAction) routeConfiguration);
      return routeConfiguration.Chain();
    }

    public void Import<T>(string prefix) where T : FubuRegistry, new()
    {
      if (Enumerable.Any<FubuRegistry.RegistryImport>((IEnumerable<FubuRegistry.RegistryImport>) this._imports, (Func<FubuRegistry.RegistryImport, bool>) (x => x.Registry is T)))
        return;
      this.Import((FubuRegistry) Activator.CreateInstance<T>(), prefix);
    }

    public void Import<T>() where T : IFubuRegistryExtension, new()
    {
      if (this._importedTypes.Contains(typeof (T)))
        return;
      new T().Configure(this);
      this._importedTypes.Add(typeof (T));
    }

    public void Import<T>(Action<T> configuration) where T : IFubuRegistryExtension, new()
    {
      T obj = new T();
      configuration(obj);
      obj.Configure(this);
      this._importedTypes.Add(typeof (T));
    }

    public void Import(FubuRegistry registry, string prefix)
    {
      this._imports.Add(new FubuRegistry.RegistryImport()
      {
        Prefix = prefix,
        Registry = registry
      });
    }

    public void ConfigureImports(Action<FubuRegistry> configuration)
    {
      this._importsConventions.Add(configuration);
    }

    public void IncludeDiagnostics(bool shouldInclude)
    {
      if (shouldInclude)
      {
        this.IncludeDiagnostics((Action<IDiagnosticsConfigurationExpression>) (config =>
        {
          config.LimitRecordingTo(50);
          DiagnosticsConfigurationExtensions.ExcludeRequests(config, (Func<CurrentRequest, bool>) (r =>
          {
            if (r.Path == null)
              return false;
            return r.Path.ToLower().StartsWith(StringExtensions.ToFormat("/{0}", new object[1]
            {
              (object) "_fubu"
            }));
          }));
        }));
        this.ConfigureImports((Action<FubuRegistry>) (i =>
        {
          if (i is DiagnosticsRegistry)
            return;
          i._diagnosticsRegistryImported = this._diagnosticsRegistryImported;
          i.IncludeDiagnostics(shouldInclude);
        }));
      }
      else
        this._diagnosticLevel = DiagnosticLevel.None;
    }

    public void IncludeDiagnostics(Action<IDiagnosticsConfigurationExpression> configure)
    {
      this._diagnosticLevel = DiagnosticLevel.FullRequestTracing;
      this.UsingObserver((IConfigurationObserver) new RecordingConfigurationObserver());
      if (!this._diagnosticsRegistryImported)
      {
        this.Import<DiagnosticsRegistry>(string.Empty);
        this._diagnosticsRegistryImported = true;
      }
      List<IRequestHistoryCacheFilter> filters = new List<IRequestHistoryCacheFilter>();
      DiagnosticsConfigurationExpression config = new DiagnosticsConfigurationExpression((IList<IRequestHistoryCacheFilter>) filters);
      configure((IDiagnosticsConfigurationExpression) config);
      this.Services((Action<IServiceRegistry>) (graph =>
      {
        graph.SetServiceIfNone<DiagnosticsIndicator>(new DiagnosticsIndicator().SetEnabled());
        graph.SetServiceIfNone<DiagnosticsConfiguration>(new DiagnosticsConfiguration()
        {
          MaxRequests = config.MaxRequests
        });
        GenericEnumerableExtensions.Each<IRequestHistoryCacheFilter>((IEnumerable<IRequestHistoryCacheFilter>) filters, (Action<IRequestHistoryCacheFilter>) (filter => graph.AddService(typeof (IRequestHistoryCacheFilter), new ObjectDef()
        {
          Type = filter.GetType(),
          Value = (object) filter
        })));
      }));
    }

    public void Configure(Action<BehaviorGraph> alteration)
    {
      this.addExplicit(alteration);
    }

    public static Assembly FindTheCallingAssembly()
    {
      StackTrace stackTrace = new StackTrace(false);
      Assembly executingAssembly = Assembly.GetExecutingAssembly();
      Assembly assembly1 = typeof (ITypeResolver).Assembly;
      Assembly assembly2 = (Assembly) null;
      for (int index = 0; index < stackTrace.FrameCount; ++index)
      {
        Assembly assembly3 = stackTrace.GetFrame(index).GetMethod().DeclaringType.Assembly;
        if (assembly3 != executingAssembly && assembly3 != assembly1)
        {
          assembly2 = assembly3;
          break;
        }
      }
      return assembly2;
    }

    private void addConvention(Action<BehaviorGraph> action)
    {
      this._conventions.Add((IConfigurationAction) new LambdaConfigurationAction(action));
    }

    private void addExplicit(Action<BehaviorGraph> action)
    {
      this._explicits.Add((IConfigurationAction) new LambdaConfigurationAction(action));
    }

    private IEnumerable<IServiceRegistry> allServiceRegistrations()
    {
      foreach (FubuRegistry.RegistryImport registryImport in this._imports)
      {
        foreach (IServiceRegistry serviceRegistry in registryImport.Registry.allServiceRegistrations())
          yield return serviceRegistry;
      }
      foreach (IServiceRegistry serviceRegistry in (IEnumerable<IServiceRegistry>) this._serviceRegistrations)
        yield return serviceRegistry;
    }

    public BehaviorGraph BuildGraph()
    {
      this.Import<AssetServicesRegistry>();
      this.Services<ModelBindingServicesRegistry>();
      this.Services<SecurityServicesRegistry>();
      this.Services<ResourcesServiceRegistry>();
      this.Services<HtmlConventionServiceRegistry>();
      this.Services<PackagingServiceRegistry>();
      this.Services<HttpStandInServiceRegistry>();
      this.Services<ViewActivationServiceRegistry>();
      this.Services<CoreServiceRegistry>();
      return this.BuildLightGraph();
    }

    public void WithTypes(Action<TypePool> configuration)
    {
      this._scanningOperations.Add(configuration);
    }

    public BehaviorGraph BuildLightGraph()
    {
      BehaviorGraph graph = new BehaviorGraph(this._observer);
      GenericEnumerableExtensions.Each<Action<TypePool>>((IEnumerable<Action<TypePool>>) this._scanningOperations, (Action<Action<TypePool>>) (x => x(this._types)));
      GenericEnumerableExtensions.Each<IConfigurationAction>(Enumerable.OfType<IConfigurationAction>((IEnumerable) this.allServiceRegistrations()), (Action<IConfigurationAction>) (x => x.Configure(graph)));
      BehaviorExtensions.Configure(this._conventions, graph);
      ObserverImporter observerImporter = new ObserverImporter(graph.Observer);
      GenericEnumerableExtensions.Each<FubuRegistry.RegistryImport>((IEnumerable<FubuRegistry.RegistryImport>) this._imports, (Action<FubuRegistry.RegistryImport>) (x =>
      {
        GenericEnumerableExtensions.Each<Action<FubuRegistry>>((IEnumerable<Action<FubuRegistry>>) this._importsConventions, (Action<Action<FubuRegistry>>) (c => c(x.Registry)));
        x.ImportInto((IChainImporter) graph);
        observerImporter.Import(x.Registry._observer);
      }));
      BehaviorExtensions.Configure(this._explicits, graph);
      BehaviorExtensions.Configure(this._policies, graph);
      BehaviorExtensions.Configure(this._systemPolicies, graph);
      return graph;
    }

    public class HandlerActionSource : ActionSource
    {
      private readonly IEnumerable<Type> _markerTypes;

      public HandlerActionSource(IEnumerable<Type> markerTypes)
        : base(new ActionMethodFilter())
      {
        GenericEnumerableExtensions.Each<Type>(markerTypes, (Action<Type>) (markerType =>
        {
          // ISSUE: object of a compiler-generated type is created
          // ISSUE: variable of a compiler-generated type
          FubuRegistry.HandlerActionSource.\u003C\u003Ec__DisplayClass32 local_0 = new FubuRegistry.HandlerActionSource.\u003C\u003Ec__DisplayClass32();
          // ISSUE: reference to a compiler-generated field
          local_0.markerType = markerType;
          CompositeFilter<Type> temp_45 = this.TypeFilters;
          CompositePredicate<Type> temp_46 = temp_45.Includes;
          ParameterExpression local_1 = Expression.Parameter(typeof (Type), "t");
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: method reference
          // ISSUE: field reference
          // ISSUE: method reference
          Expression<Func<Type, bool>> temp_96 = Expression.Lambda<Func<Type, bool>>((Expression) Expression.AndAlso((Expression) Expression.Call((Expression) null, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (StringExtensions.IsNotEmpty)), new Expression[1]
          {
            (Expression) Expression.Property((Expression) local_1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Type.get_Namespace)))
          }), (Expression) Expression.Call((Expression) Expression.Property((Expression) local_1, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Type.get_Namespace))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.StartsWith)), new Expression[1]
          {
            (Expression) Expression.Property((Expression) Expression.Field((Expression) Expression.Constant((object) local_0), FieldInfo.GetFieldFromHandle(__fieldref (FubuRegistry.HandlerActionSource.\u003C\u003Ec__DisplayClass32.markerType))), (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (Type.get_Namespace)))
          })), new ParameterExpression[1]
          {
            local_1
          });
          CompositePredicate<Type> temp_97 = temp_46 + temp_96;
          temp_45.Includes = temp_97;
        }));
        ActionMethodFilter methodFilters = this.MethodFilters;
        CompositePredicate<MethodInfo> includes = methodFilters.Includes;
        ParameterExpression parameterExpression = Expression.Parameter(typeof (MethodInfo), "m");
        // ISSUE: method reference
        // ISSUE: method reference
        Expression<Func<MethodInfo, bool>> expression = Expression.Lambda<Func<MethodInfo, bool>>((Expression) Expression.Equal((Expression) Expression.Property((Expression) parameterExpression, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (MemberInfo.get_Name))), (Expression) Expression.Constant((object) "Execute", typeof (string)), false, (MethodInfo) MethodBase.GetMethodFromHandle(__methodref (string.op_Equality))), new ParameterExpression[1]
        {
          parameterExpression
        });
        CompositePredicate<MethodInfo> compositePredicate = includes + expression;
        methodFilters.Includes = compositePredicate;
        this._markerTypes = markerTypes;
      }

      public bool Equals(FubuRegistry.HandlerActionSource other)
      {
        if (object.ReferenceEquals((object) null, (object) other))
          return false;
        if (object.ReferenceEquals((object) this, (object) other))
          return true;
        if (Enumerable.Count<Type>(other._markerTypes) != Enumerable.Count<Type>(this._markerTypes))
          return false;
        for (int index = 0; index < Enumerable.Count<Type>(this._markerTypes); ++index)
        {
          if (Enumerable.ElementAt<Type>(other._markerTypes, index) != Enumerable.ElementAt<Type>(this._markerTypes, index))
            return false;
        }
        return true;
      }

      public override bool Equals(object obj)
      {
        if (object.ReferenceEquals((object) null, obj))
          return false;
        if (object.ReferenceEquals((object) this, obj))
          return true;
        if (obj.GetType() != typeof (FubuRegistry.HandlerActionSource))
          return false;
        else
          return this.Equals((FubuRegistry.HandlerActionSource) obj);
      }

      public override int GetHashCode()
      {
        if (this._markerTypes == null)
          return 0;
        else
          return this._markerTypes.GetHashCode();
      }
    }

    public class RegistryImport
    {
      public string Prefix { get; set; }

      public FubuRegistry Registry { get; set; }

      public void ImportInto(IChainImporter graph)
      {
        graph.Import(this.Registry.BuildLightGraph(), (Action<BehaviorChain>) (b =>
        {
          b.PrependToUrl(this.Prefix);
          b.Origin = this.Registry.Name;
        }));
      }
    }
  }
}
