using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebBoard.Logic.UnitOfWork.Interface;
using WebBoard.DataAccess.EntityFramework.DbContextModel;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebBoard.Controllers
{
    [Route("api/[controller]")]
    public class WebBoardPostController : Controller
    {
        private ILogicUnitOfWork LogicUnitOfWork { get; set; }
        public WebBoardPostController(ILogicUnitOfWork LogicUnitOfWork)
        {
            this.LogicUnitOfWork = LogicUnitOfWork;
        }

        [HttpGet("Get")]
        public async Task<Post> Get(int postId)
        {
            return await LogicUnitOfWork.PostService.GetPost(postId);
        }


        [HttpGet("GetPostList")]
        public async Task<List<Post>> GetPostList()
        {
            try {
                var returnData = await LogicUnitOfWork.PostService.GetPostList();
                return returnData;
            }
            catch (Exception e) {
                throw e;
            }
        }

        [HttpPost("CreatePost")]
        public async Task<bool> CreatePost([FromBody]Post postData)
        {
            try {
                postData.PostTimeStamp = DateTime.Now;
                await LogicUnitOfWork.PostService.CreatePost(postData);
                return true;
            }
            catch (Exception e) {

                throw e;

            }
        }

        public class PostDataModel {

        }



    }
}
