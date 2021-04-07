using lol_decay_api.Helper_Classes;
using lol_decay_api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.Json;
using System.Threading.Tasks;
using static lol_decay_api.Helper_Classes.MailService;

namespace lol_decay_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailServiceController : Controller
    {
        private readonly MailService _MailService;
        public MailServiceController(MailService MailService)
        {
            _MailService = MailService;
        }

        [HttpGet]
        public ActionResult Get()
        {
            return Ok("Test");
        }

        [HttpPost]
        public ActionResult Post(JsonElement js)
        {
            return Ok(_MailService.MailSender(JsonConvert.DeserializeObject<EmailClass>(js.GetRawText())).ToString());
        }

    }
}
