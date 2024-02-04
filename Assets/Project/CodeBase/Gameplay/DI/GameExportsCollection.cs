using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Core.DI;
using CodeBase.Core.Factories;
using CodeBase.Core.Services;

namespace CodeBase.Gameplay.DI
{
    public sealed class GameExportsCollection : ExportsCollectionBase
    {
        protected override Type[] ExportedTypes => new[]
        {
            typeof(IService),
            typeof(IFactory)
        };
        
        public override IReadOnlyList<Type> GetExportedTypes()
        {
            var types = Assembly.GetExecutingAssembly().ExportedTypes
                .Where(CanExportType)
                .ToList();
            types.Add(typeof(Initializer));

            return types;
        }
    }
}