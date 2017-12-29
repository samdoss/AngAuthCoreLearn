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
    public class WebBoardCommentController : Controller
    {
        private ILogicUnitOfWork LogicUnitOfWork { get; set; }
        public WebBoardCommentController(ILogicUnitOfWork LogicUnitOfWork)
        {
            this.LogicUnitOfWork = LogicUnitOfWork;
        }


        [HttpGet("GetComment")]
        public async Task<List<Comment>> GetComment(int postId)
        {
            return await LogicUnitOfWork.CommentService.GetComment(postId);
        }


        [HttpPost("CreateComment")]
        public async Task<bool> CreateComment(Comment commentDat)
        {
            commentDat.CommentTimeStamp = DateTime.Now;
            return await LogicUnitOfWork.CommentService.CreateComment(commentDat);
        }






    }
}
