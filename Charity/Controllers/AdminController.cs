
using System.Linq;
using System.Web.Mvc;
using Charity.Models;

namespace Charity.Controllers
{
    public class AdminController : Controller
    {
        private CharityDBEntities database = new CharityDBEntities();

        // GET: Admin
        public ActionResult Index()
        {
            // Chưa đăng nhập
            if (Session["UserId"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Không có quyền Admin
            if (Session["Role"] == null ||
                Session["Role"].ToString() != "Admin")
            {
                return RedirectToAction("Index", "Home");
            }

            // Thống kê từ database
            ViewBag.TotalCampaigns = database.Campaigns.Count();
            ViewBag.TotalUsers = database.Users.Count();
            ViewBag.TotalDonations = database.Donations.Count();

            ViewBag.TotalAmount = database.Donations
                .Select(d => (decimal?)d.Amount)
                .DefaultIfEmpty(0)
                .Sum() ?? 0;

            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                database.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
