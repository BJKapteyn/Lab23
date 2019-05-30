using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Lab23.Models;

namespace Lab23.Controllers
{
    public class HomeController : Controller
    {
        ShopDBEntities1 db = new ShopDBEntities1();
        

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Registeration()
        {
            return View();
        }

        public ActionResult Register(User u)
        {
            if (u.Pass.Length < 8)
            {
                Session["PassError"] = "Password must be more than 8 characters";
                return RedirectToAction("Registeration");
            }
            else if (u.Cash <= 0)
            {
                Session["CashError"] = "You don't want to owe me money";
            }
            else
            {
                Session["PassError"] = "";
                Session["CashError"] = "";
                db.Users.Add(u);
                db.SaveChanges();
            }
            return View();
        }

        //if you want to visit the page without action
        public ActionResult Login()

        {
            return View();
        }

        //overload for when information is being submitted
        [HttpPost]
        public ActionResult Login(LogUser u)
        {
            List<User> users = db.Users.ToList();
            User user = new User();
            int id = 0;

            for (int i = 0; i < users.Count; i++)
            {
                if(u.UserName == users[i].UserName)
                {
                    id = users[i].id;
                    user = users[i];
                    Session["LoginUsernameError"] = "";
                    if (u.Pass == user.Pass)
                    {
                        Session["User"] = user;
                        Session["LoginUsernameError"] = "";
                        return View();
                    }
                    else
                    {
                        Session["LoginPassError"] = "Ya Blew it";
                    }
                }
                else
                {
                    continue;
                }
                Session["LoginUsernameError"] = "YA BLEW IT";
            }
            return RedirectToAction("Index");

        }

        public ActionResult Shop()
        {
            return View();
        }


        public ActionResult Purchase(int id, int quantity, int userId)
        {
            if (quantity != null)
            {
                Item item = db.Items.Find(id);
                User u = db.Users.Find(userId);
                double price = (double)item.Price * quantity;
                if (item.Quantity > quantity)
                {
                    Session["QuantityError"] = "";
                    item.Quantity -= quantity;
                    db.SaveChanges();
                }
                else
                {
                    Session["QuantityError"] = "Not enough in stock sorry";
                }
                if (u.Cash > price)
                {
                    u.Cash -= price;
                    db.SaveChanges();
                    Session["User"] = u;
                }
                else
                {
                    Session["PriceError"] = "You don't have enough cash";
                }
            }
            else
            {
                Session["QuantityError"] = "Gotta enter a quantity";
            }
            return RedirectToAction("Shop");
        }
    }
}