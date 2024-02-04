using System.Threading.Tasks;

namespace Project.CodeBase.Core.Services
{
    public interface IInitializable
    {
        Task<bool> Initialize();
    }
}