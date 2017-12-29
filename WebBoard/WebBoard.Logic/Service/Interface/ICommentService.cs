using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.DbContextModel;

namespace WebBoard.Logic.Service.Interface
{
    public interface ICommentService
    {
        //Task<ListManagerDashBoardDto> GetListManagerDashBoardByCompanyAsync(int CompanySurrKey);
        Task<List<Comment>> GetComment(int postId);
        Task<bool> CreateComment(Comment commentDat);
    }
}
