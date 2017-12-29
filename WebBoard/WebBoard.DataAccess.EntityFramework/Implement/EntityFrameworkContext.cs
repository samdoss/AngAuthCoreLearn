using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using WebBoard.DataAccess.EntityFramework.DbContextModel;
using WebBoard.DataAccess.EntityFramework.Interface;

namespace WebBoard.DataAccess.EntityFramework.Implement
{
    public class EntityFrameworkContext : IEntityFrameworkContext
    {
        public DbContext GetConnection() => new webboarddbContext();
    }
}
