using System.Threading.Tasks;

namespace CodeBase.Core.Services
{
    public interface IInitializable
    {
        Task<bool> Initialize();
    }
}