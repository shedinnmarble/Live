using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TV.Replays.Model;

namespace TV.Replays.WebApi.Models
{
    public class PlayerViewModel
    {
        public Player Player { get; set; }
        public HttpPostedFileBase UploadFile { get; set; }
    }
}