﻿using Chamou.Web.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Web.Http;

namespace Chamou.Web.ApiControllers
{
    public class AttendantsController : ApiController
    {
        private ChamouContext db = new ChamouContext();

        public IHttpActionResult GetAttendant(int id)
        {
            var data = $"{id};{new Random().Next(20).ToString()}";
            var client = new TcpClient("ypushtcp.cloudapp.net", 8081);
            var stream = client.GetStream();
            stream.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            client.Close();

            return Ok();
        }
    }
}