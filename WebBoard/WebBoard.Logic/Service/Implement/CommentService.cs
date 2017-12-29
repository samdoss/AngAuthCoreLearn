using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.DbContextModel;
using WebBoard.DataAccess.EntityFramework.Interface;
using WebBoard.Logic.Service.Interface;

namespace WebBoard.Logic.Service.Implement
{
    public class CommentService : ICommentService
    {

        private IEntityUnitOfWork EntityUnitOfWork { get; set; }
        public CommentService(IEntityUnitOfWork EntityUnitOfWork)
        {

            this.EntityUnitOfWork = EntityUnitOfWork;
        }

       
        public async Task<List<Comment>> GetComment(int postId)
        {
            return await EntityUnitOfWork.CommentRepository.GetAll(x => x.PostId == postId).ToListAsync();
        }

        public async Task<bool> CreateComment(Comment commentData)
        {
            await EntityUnitOfWork.CommentRepository.AddAsync(commentData);
            await EntityUnitOfWork.SaveAsync();

            return true;
        }
    }
}
