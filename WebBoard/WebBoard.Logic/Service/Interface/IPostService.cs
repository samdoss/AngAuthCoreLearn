using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebBoard.DataAccess.EntityFramework.DbContextModel;

namespace WebBoard.Logic.Service.Interface
{
    public interface IPostService
    {
        Task<Post> GetPost(int postId);
        Task<List<Post>> GetPostList();
        Task<bool> CreatePost(Post postData);
    }
}
