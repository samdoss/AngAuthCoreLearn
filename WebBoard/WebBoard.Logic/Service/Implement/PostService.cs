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
    public class PostService : IPostService

    {
      
        private IEntityUnitOfWork EntityUnitOfWork { get; set; }
        public PostService(IEntityUnitOfWork EntityUnitOfWork)
        {
            this.EntityUnitOfWork = EntityUnitOfWork;
        }

         
        public async Task<Post> GetPost(int postId)
        {
            return await EntityUnitOfWork.PostRepository.GetSingleAsync(x => x.PostId == postId);
        }

        public async Task<List<Post>> GetPostList()
        {
            return await EntityUnitOfWork.PostRepository.GetAll().ToListAsync();
        }

        public async Task<bool> CreatePost(Post postData)
        {
            
            await EntityUnitOfWork.PostRepository.AddAsync(postData);
            await EntityUnitOfWork.SaveAsync();

            return true;
        }

        
    }
}
