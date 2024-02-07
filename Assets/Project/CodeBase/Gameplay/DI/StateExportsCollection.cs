using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CodeBase.Core.DI;
using CodeBase.Core.StateMachine.Interfaces;

namespace CodeBase.Gameplay.DI
{
    public sealed class StateExportsCollection : ExportsCollectionBase
    {
        protected override Type[] ExportedTypes => new[]
        {
            typeof(IExitableState)
        };
        
        public override IReadOnlyList<Type> GetExportedTypes()
        {
            var types = Assembly.GetExecutingAssembly().ExportedTypes
                .Where(CanExportType)
                .ToList();

            return types;
        }
    }
}