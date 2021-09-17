using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebAppRestaurantDemoApp.Models;
using WebAppRestaurantDemoApp.Repositories;
using WebAppRestaurantDemoApp.ViewModel;

namespace WebAppRestaurantDemoApp.Controllers
{
    public class HomeController : Controller
    {
        private RestaurantDBEntities objRestaurantDBEntities;
        // GET: Home
        public HomeController()
        {
            objRestaurantDBEntities = new RestaurantDBEntities();
        }
        public ActionResult Index()
        {
            //get all the repositories here
            CustomerRepository objCustomers = new CustomerRepository();
            ItemRepository objItems = new ItemRepository();
            PaymentTypeRepository objPayments = new PaymentTypeRepository();

            // get all the ViewModel
            var objAllViewModel = new Tuple<IEnumerable<SelectListItem>,IEnumerable<SelectListItem>,IEnumerable<SelectListItem>>
                (objCustomers.GetAllCustomers(),objItems.GetAllItems(),objPayments.GetAllPaymentType());
            return View(objAllViewModel);
        }

        [HttpGet]
        public JsonResult getItemUnitPrice(int? itemId)
        {
            decimal UnitPrice = objRestaurantDBEntities.Items.Single(x => x.ItemId == itemId).ItemPrice;
            return Json(UnitPrice, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Index(OrderViewModel objOrderViewModel)
        {
            // 10th steps : add those data into databse
            OrderRepository objOrderRepotory = new OrderRepository();
            objOrderRepotory.AddOrder(objOrderViewModel);
            return Json ("Order Successfully Created", JsonRequestBehavior.AllowGet);
        }
            
    }
}