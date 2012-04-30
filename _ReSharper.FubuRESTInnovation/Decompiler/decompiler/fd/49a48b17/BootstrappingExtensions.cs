// Type: FubuMVC.StructureMap.BootstrappingExtensions
// Assembly: FubuMVC.StructureMap, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// Assembly location: C:\dev\git projectsx\FubuRESTInnovation\packages\FubuMVC.References.0.9.5.817\lib\net40\FubuMVC.StructureMap.dll

using FubuMVC.Core;
using FubuMVC.Core.Bootstrapping;
using StructureMap;
using System;

namespace FubuMVC.StructureMap
{
  public static class BootstrappingExtensions
  {
    public static FubuApplication StructureMapObjectFactory(this IContainerFacilityExpression expression)
    {
      return BootstrappingExtensions.StructureMap(expression, (Func<IContainer>) (() => ObjectFactory.Container));
    }

    public static FubuApplication StructureMapObjectFactory(this IContainerFacilityExpression expression, Action<IInitializationExpression> structureMapBootstrapper)
    {
      return BootstrappingExtensions.StructureMap(expression, (Func<IContainer>) (() =>
      {
        ObjectFactory.Initialize(structureMapBootstrapper);
        return ObjectFactory.Container;
      }));
    }

    public static FubuApplication StructureMap(this IContainerFacilityExpression expression, IContainer container)
    {
      return BootstrappingExtensions.StructureMap(expression, (Func<IContainer>) (() => container));
    }

    public static FubuApplication StructureMap(this IContainerFacilityExpression expression, Func<IContainer> createContainer)
    {
      return expression.ContainerFacility((Func<IContainerFacility>) (() => (IContainerFacility) new StructureMapContainerFacility(createContainer())));
    }
  }
}
