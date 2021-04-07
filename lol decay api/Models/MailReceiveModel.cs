using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace lol_decay_api.Models
{
    public class MailReceiveModel
    {
        public MailReceiveModel(string email)
        {
            Email = email;
        }

        public string Email { get; set; }
    }
}
