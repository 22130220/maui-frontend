using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiFrontend.Models
{
    public class UserModel
    {
        public int Id {  get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
    }
}
