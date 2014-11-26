using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using TV.Replays.IDAL;
using TV.Replays.Model;

namespace TV.Replays.DAL
{
    public class PlayerDal : IPlayerDal
    {
        private static readonly string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
        private static readonly string fileName = Path.Combine(path, "player.xml");
        private static readonly string root = "playerList";

        public PlayerDal()
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            if (!File.Exists(fileName))
            {
                var xDoc = new XDocument(new XElement(root));
                xDoc.Save(fileName);
            }
        }

        public void Insert(Player player)
        {
        start:
            try
            {
                var xDoc = XDocument.Load(fileName);
                var element = new XElement("player",
                new XAttribute("id", player.Id),
                new XElement("name", player.Name),
                new XElement("icon", player.Icon),
                new XElement("game", player.Game),
                new XElement("categories", string.Join(",", player.Categories)),
                new XElement("recommend", player.Recommend),
                new XElement("level", player.Level),
                    //new XElement("gender", player.Gender),
                new XElement("description", player.Description),
                new XElement("liveRooms")
                );

                foreach (var item in player.LiveRooms)
                {
                    element.Element("liveRooms").Add(new XElement("liveRoom", new XElement("name", item.Name), new XElement("url", item.Url)));
                }
                xDoc.Root.Add(element);
                xDoc.Save(fileName);
            }
            catch (XmlException)
            {
                var xDoc = new XDocument(new XElement(root));
                xDoc.Save(fileName);
                goto start;
            }

        }

        public void Delete(string id)
        {
            var xDoc = XDocument.Load(fileName);
            var xe = from item in xDoc.Root.Elements("player")
                     where item.Attribute("id").Value == id
                     select item;
            xe.Remove();
            xDoc.Save(fileName);
        }

        public void Update(Player player)
        {
            var xDoc = XDocument.Load(fileName);
            var xe = (from item in xDoc.Root.Elements("player")
                      where item.Attribute("id").Value == player.Id
                      select item).SingleOrDefault();

            if (xe == null)
                return;

            xe.SetAttributeValue("id", player.Id);
            xe.Element("name").SetValue(player.Name);
            xe.Element("icon").SetValue(player.Icon ?? "");
            xe.Element("game").SetValue(player.Game);
            xe.Element("categories").SetValue(string.Join(",", player.Categories));
            xe.Element("recommend").SetValue(player.Recommend);
            xe.Element("level").SetValue(player.Level);
            //xe.Element("gender").SetValue(player.Gender);
            xe.Element("description").SetValue(player.Description ?? "");
            xe.Element("liveRooms").Elements().Remove();
            foreach (var item in player.LiveRooms)
            {
                xe.Element("liveRooms").Add(new XElement("liveRoom", new XElement("name", item.Name), new XElement("url", item.Url)));
            }

            xDoc.Save(fileName);
        }

        public Player Get(string id)
        {
            var xDoc = XDocument.Load(fileName);
            var xe = xDoc.Root.Elements("player").Where(a => a.Attribute("id").Value == id).SingleOrDefault();

            if (xe == null)
                return null;

            Player p = CreatePlayer(xe);
            return p;
        }

        private Player CreatePlayer(XElement xe)
        {
            Player p = new Player();
            p.Id = xe.Attribute("id").Value;
            p.Name = xe.Element("name").Value;
            p.Icon = xe.Element("icon").Value;
            p.Game = xe.Element("game").Value;
            //p.Gender = xe.Element("gender").Value;
            p.Categories = xe.Element("categories").Value.Split(',');
            p.Recommend = Convert.ToBoolean(xe.Element("recommend").Value);
            p.Level = Convert.ToInt32(xe.Element("level").Value);
            p.Description = xe.Element("description").Value;
            p.LiveRooms = new List<LiveRoom>();
            foreach (var item in xe.Element("liveRooms").Elements("liveRoom"))
            {
                LiveRoom room = new LiveRoom();
                room.Url = item.Element("url").Value;
                room.Name = item.Element("name").Value;
                p.LiveRooms.Add(room);
            }
            return p;
        }

        public IEnumerable<Player> Get()
        {
            var xDoc = XDocument.Load(fileName);
            var element = xDoc.Root.Elements("player");

            if (element == null)
                return null;

            List<Player> list = new List<Player>();
            foreach (var xe in element)
            {
                list.Add(CreatePlayer(xe));
            }
            return list;
        }


        public IEnumerable<Player> Get(int pageIndex, int pageSize, out int total)
        {
            total = Get().Count();
            return Get().Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

    }
}
