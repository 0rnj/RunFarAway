using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.CodeBase.Core.DI
{
    public abstract class ExportsCollectionBase
    {
        protected abstract Type[] ExportedTypes { get; }

        public abstract IReadOnlyList<Type> GetExportedTypes();

        protected bool CanExportType(Type type)
        {
            return type.IsAbstract == false && ExportedTypes.Any(t => IsSubclassOf(type, t));
        }

        private bool IsSubclassOf(Type type, Type abstractType)
        {
            return type != abstractType && abstractType.IsAssignableFrom(type);
        }
    }
}