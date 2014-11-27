using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TV.Replays.Common;
using TV.Replays.Contract;
using TV.Replays.DAL;
using TV.Replays.IDAL;
using TV.Replays.Model;
using TV.Replays.Model.ViewModel;
using TV.Replays.WebApi.Models;

namespace TV.Replays.WebApi.Controllers
{
    [AuthenticationFilter]
    public class PlayerEditController : Controller
    {
        public IPlayerDal _playerDal;
        public PlayerEditController()
        {
            _playerDal = new PlayerDal();
        }

        public ActionResult Index(int page = 1, string category = "", string tv = "", bool isOnline = false, bool recommend = false, bool isDesc = true)
        {
            int pageIndex = page;
            int pageSize = 15;
            int count = 0;

            IEnumerable<PlayerEditViewModel> vmList;
            using (ChannelFactory<ILiveService> channelFactory = new ChannelFactory<ILiveService>("dota2Client"))
            {
                var channel = channelFactory.CreateChannel();
                vmList = channel.GetPlayerEditViewModels();
                count = vmList.Count();
            }

            if (!String.IsNullOrEmpty(category))
                vmList = vmList.Where(a => a.Categories.Contains(category));
            if (!String.IsNullOrEmpty(tv))
                vmList = vmList.Where(a => a.TVNames.Contains(tv));
            if (isOnline)
                vmList = vmList.Where(a => a.IsOnline == true);
            if (recommend)
                vmList = vmList.Where(a => a.IsRecommend == true);
            if (isDesc)
                vmList = vmList.OrderByDescending(a => a.Level);

            vmList = vmList
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            ViewBag.CurrentIndex = pageIndex;
            ViewBag.TotalPages = (count + pageSize - 1) / pageSize;
            ViewBag.NumberOfPages = count;
            return View(vmList);
        }

        public ActionResult Create()
        {
            return View(new Player());
        }

        [HttpPost]
        public ActionResult Create(Player player, HttpPostedFileBase UploadFile)
        {
            if (ModelState.IsValid)
            {
                if (UploadFile != null)
                {
                    string url = UploadImage(UploadFile);
                    player.Icon = url;
                }
                var stream = Request.InputStream;
                if (player.Categories == null)
                    player.Categories = new string[0];
                _playerDal.Insert(player);
                return RedirectToAction("Index");
            }
            else
            {
                return View(player);
            }
        }

        public ActionResult Edit(string id)
        {
            var player = _playerDal.Get(id);
            return View(player);
        }

        [HttpPost]
        public ActionResult Edit(Player player, HttpPostedFileBase UploadFile)
        {
            if (ModelState.IsValid)
            {
                if (UploadFile != null)
                {
                    string url = UploadImage(UploadFile);
                    player.Icon = url;
                }
                _playerDal.Update(player);
                return RedirectToAction("Index");
            }
            else
            {
                return View(player);
            }
        }

        public ActionResult Delete(string id)
        {
            _playerDal.Delete(id);
            return RedirectToAction("Index");
        }

        public ActionResult EditLevel(string id, int level)
        {
            var player = _playerDal.Get(id);
            player.Level = level;
            _playerDal.Update(player);
            string url = Request.UrlReferrer.AbsoluteUri;
            return Redirect(url);
        }

        public ActionResult Recommend(string id)
        {
            var player = _playerDal.Get(id);
            player.Recommend = true;
            _playerDal.Update(player);
            return RedirectToAction("Index");
        }

        public ActionResult CancelRecommend(string id)
        {
            var player = _playerDal.Get(id);
            player.Recommend = false;
            _playerDal.Update(player);
            return RedirectToAction("Index");
        }

        private string UploadImage(HttpPostedFileBase file)
        {
            string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "PlayerIcon");
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            string md5 = Md5Helper.MD5FromFile(file.InputStream);
            string fileName = Path.Combine(dir, md5 + ".jpg");

            if (!System.IO.File.Exists(fileName))
                file.SaveAs(fileName);

            string url = "/live/playerIcon/" + md5 + ".jpg";

#if DEBUG
            url = url.TrimStart("/live".ToArray());
#endif
            return url;

        }

        public ActionResult ServerShutdown()
        {
            WcfServiceInitator.Close("TV.Replays.Dota2Hosting");
            return Content("closed");
        }
    }
}
