using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.DbContextModel;

namespace WebBoard.DataAccess.EntityFramework.Interface
{
    public interface IEntityUnitOfWork
    {
        IEntityFrameworkRepository<Post> PostRepository { get; set; }
        IEntityFrameworkRepository<Comment> CommentRepository { get; set; }
        
        Task<int> SaveAsync();
    }
}
