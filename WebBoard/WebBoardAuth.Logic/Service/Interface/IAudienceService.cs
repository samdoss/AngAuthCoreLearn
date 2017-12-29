using WebBoardAuth.DataAccess.Sql.Entities;
using WebBoardAuth.Logic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace WebBoardAuth.Logic.Service.Interface
{
    public interface IAudienceService
    {
        Task<Audience> AddAudience(AudienceDto doc);
        Task<Audience> FindAudience(string clientId);
        Task<List<Audience>> GetAllAudience();
    }
}
