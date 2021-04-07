using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lol_decay_api.Helper_Classes
{
    public class EmailClass
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class UserClass : IUserClass
    {
        public string UserName { get; set; }
        public string Pass { get; set; }
    }
    public interface IUserClass
    {
        string UserName { get; set; }
        string Pass { get; set; }
    }

}
