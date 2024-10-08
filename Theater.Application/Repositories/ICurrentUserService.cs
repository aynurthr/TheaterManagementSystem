﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Application.Repositories
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        Task<string> GetUserNameAsync();
    }
}
