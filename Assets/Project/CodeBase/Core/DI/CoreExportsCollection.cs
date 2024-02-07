using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Core.Factories;
using CodeBase.Core.Services;

namespace CodeBase.Core.DI
{
    public class CoreExportsCollection : ExportsCollectionBase
    {
        protected override Type[] ExportedTypes => new[]
        {
            typeof(IFactory),
            typeof(IService)
        };
        
        public override IReadOnlyList<Type> GetExportedTypes()
        {
            var types = Assembly.GetExecutingAssembly()
                .ExportedTypes
                .Where(CanExportType)
                .ToList();

            return types;
        }
    }
}