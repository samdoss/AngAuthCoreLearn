using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.DbContextModel;
using WebBoard.DataAccess.EntityFramework.Interface;

namespace WebBoard.DataAccess.EntityFramework.Implement
{
    public class EntityUnitOfWork : IEntityUnitOfWork
    {
        private readonly DbContext Context;
        public EntityUnitOfWork(IEntityFrameworkContext context)
        {
            Context = context.GetConnection();
        }

        #region   Post 
        private IEntityFrameworkRepository<Post> IPostRepository;
        public IEntityFrameworkRepository<Post> PostRepository
        {
            get { return IPostRepository ?? (IPostRepository = new EntityFrameworkRepository<Post>(Context)); }
            set { IPostRepository = value; }
        }
        #endregion

        #region   Comment 
        private IEntityFrameworkRepository<Comment> ICommentRepository;
        public IEntityFrameworkRepository<Comment> CommentRepository
        {
            get { return ICommentRepository ?? (ICommentRepository = new EntityFrameworkRepository<Comment>(Context)); }
            set { ICommentRepository = value; }
        }
        #endregion
 

        public async Task<int> SaveAsync()
        {
            
            return await Context.SaveChangesAsync();
        }
    }
}
