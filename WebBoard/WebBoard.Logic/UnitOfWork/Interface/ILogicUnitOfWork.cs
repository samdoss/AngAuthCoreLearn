using System;
using System.Collections.Generic;
using System.Text;
using WebBoard.Logic.Service.Interface;

namespace WebBoard.Logic.UnitOfWork.Interface
{
    public interface ILogicUnitOfWork
    {
        ICommentService CommentService { get; set; }
        IPostService PostService { get; set; }
    }
}
