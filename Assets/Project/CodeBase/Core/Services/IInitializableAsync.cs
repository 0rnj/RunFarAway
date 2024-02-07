using System.Threading.Tasks;

namespace CodeBase.Core.Services
{
    public interface IInitializableAsync
    {
        int InitOrder { get; }
        
        Task<bool> Initialize();
    }
}