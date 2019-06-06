using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Lab23.Models;

namespace Lab23.Controllers
{
    public class UserItemsController : Controller
    {
        public static ShopDBEntities1 db = new ShopDBEntities1();

        public ActionResult Index()
        {
            List<UserItem> userItems = db.UserItems.ToList();
            List<UserItem> currentUserItems = new List<UserItem>();
            User user = (User)Session["User"];
            foreach (UserItem u in userItems)
            {
                if (u.UserID == user.id)
                {
                    currentUserItems.Add(u);
                }
            }
            return View(currentUserItems);
        }

        public ActionResult Details(int? id)
        {
            Item item = db.UserItems.Find(id).Item;
            return View(item);
        }

        public ActionResult Delete(int id, int userId)
        {
            UserItem item = db.UserItems.Find(id);
            Item Copy = item.Item;
            User currentU = db.Users.Find(userId);
            Session["User"] = currentU;
            currentU.Cash += Copy.Price;
            db.UserItems.Remove(item);

            db.SaveChanges();
            return View(Copy);
        }

    }
}
