using GameChien.Hubs;
using GameChien.Models.Ext;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GameChien.Controllers.Webhook
{
    public class NotifyWebhookController : ApiController
    {
        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] NotifyToPlayerModel notifyToPlayer)
        {
            if (Request.Headers.TryGetValues("apikey", out var apikey) && (apikey?.FirstOrDefault() ?? "").Equals(ConfigurationManager.AppSettings["apikey"]))
            {
                RealtimeHub.pushNotifyToPlayer(notifyToPlayer.AccountName, notifyToPlayer);
            }
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}