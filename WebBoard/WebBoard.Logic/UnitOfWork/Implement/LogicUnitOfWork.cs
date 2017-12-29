using System;
using System.Collections.Generic;
using System.Text;
using WebBoard.DataAccess.EntityFramework.Interface;
using WebBoard.Logic.Service.Implement;
using WebBoard.Logic.Service.Interface;
using WebBoard.Logic.UnitOfWork.Interface;

namespace WebBoard.Logic.UnitOfWork.Implement
{
    public class LogicUnitOfWork : ILogicUnitOfWork
    {
        private IEntityUnitOfWork EntityUnitOfWork { get; set; }
       
        public LogicUnitOfWork( IEntityUnitOfWork EntityUnitOfWork)
        {
            this.EntityUnitOfWork = EntityUnitOfWork;
           
        }


        //Service Comment
        private ICommentService ICommentService { get; set; }
        public ICommentService CommentService
        {
            get { return ICommentService ?? (ICommentService = new CommentService(EntityUnitOfWork)); }
            set { ICommentService = value; }
        }

        //Service Post
        private IPostService IPostService { get; set; }
         
        public IPostService PostService
        {
            get => IPostService ?? (IPostService = new PostService(EntityUnitOfWork));
            set { IPostService = value; }
        }
    }
}
