using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Chamou.Web.ApiControllers
{
    public class PushController : ApiController
    {
        [EnableCors("*", "*", "*")]
        public IHttpActionResult Get(int id, string message = null)
        {
            var buffer = new byte[256];
            var data = $"{id};{message}";
            var client = new TcpClient("ypushtcp.cloudapp.net", 8081);
            var stream = client.GetStream();

            stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            int i = stream.Read(buffer, 0, buffer.Length);
            string responseMessage = Encoding.ASCII.GetString(buffer, 0, i);
            client.Close();

            if (!responseMessage.ToLower().Contains("erro"))
            {
                return Ok(new { Message = responseMessage });
            }

            return BadRequest(responseMessage);
        }
    }
}
