using System;
using System.Collections.Generic;

namespace WebBoard.DataAccess.EntityFramework.DbContextModel
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string CommentDetail { get; set; }
        public DateTimeOffset CommentTimeStamp { get; set; }
        public int PostId { get; set; }

        public Comment CommentNavigation { get; set; }
        public Post Post { get; set; }
        public Comment InverseCommentNavigation { get; set; }
    }
}
