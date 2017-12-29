using WebBoardAuth.DataAccess.Sql.Entities;
using WebBoardAuth.Logic.Models;
using System.Threading.Tasks;

namespace WebBoardAuth.Logic.Service.Interface
{
    public interface IClientService
    {
        Task AddClient(ClientDto model);
        Task<Client> FindClient(string clientId);
    }
}
