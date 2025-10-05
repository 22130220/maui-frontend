using MauiFrontend.Http;
using MauiFrontend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Services
{
    public class UserService : BaseService<UserModel>
    {
        public UserService(Https https) : base(https)
        {
        }
    }
}
