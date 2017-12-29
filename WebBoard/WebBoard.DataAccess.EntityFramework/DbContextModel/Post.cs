using System;
using System.Collections.Generic;

namespace WebBoard.DataAccess.EntityFramework.DbContextModel
{
    public partial class Post
    {
        public Post()
        {
            Comment = new HashSet<Comment>();
        }

        public int PostId { get; set; }
        public string PostSubject { get; set; }
        public string PostDetail { get; set; }
        public DateTimeOffset PostTimeStamp { get; set; }

        public ICollection<Comment> Comment { get; set; }
    }
}
