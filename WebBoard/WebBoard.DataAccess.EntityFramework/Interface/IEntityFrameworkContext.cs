﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebBoard.DataAccess.EntityFramework.Interface
{
    public interface IEntityFrameworkContext
    {
        DbContext GetConnection();
    }
}
