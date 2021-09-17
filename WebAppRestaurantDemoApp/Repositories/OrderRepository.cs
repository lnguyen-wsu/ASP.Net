using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebAppRestaurantDemoApp.Models;
using WebAppRestaurantDemoApp.ViewModel;

namespace WebAppRestaurantDemoApp.Repositories
{
    public class OrderRepository
    {
        private RestaurantDBEntities objRestaurantDBEntities;
        public OrderRepository()
        {
            objRestaurantDBEntities = new RestaurantDBEntities();
        }

        public bool AddOrder(OrderViewModel objOrderViewModel)
        {
            Order objOrder = new Order();
            objOrder.CustomerId = objOrderViewModel.CustomerId;
            objOrder.FinalTotal = objOrderViewModel.FinalTotal;
            objOrder.OrderDate = DateTime.Now;
            objOrder.OrderNumber = String.Format("{0:ddmmmyyyyhhmmss}", DateTime.Now);
            objOrder.PaymentTypeId = objOrderViewModel.PaymentTypeId;
            objRestaurantDBEntities.Orders.Add(objOrder);
            objRestaurantDBEntities.SaveChanges();
            int OrderId = objOrder.OrderId;

            // Now go deeper to save in order detail
            foreach (var item in objOrderViewModel.ListOfOrderDetailViewModel)
            {
                OrderDetail objDetail = new OrderDetail();
                objDetail.OrderId = OrderId;
                objDetail.Quantity = item.Quantity;
                objDetail.UnitPrice = item.UnitPrice;
                objDetail.Discount = item.Discount;
                objDetail.Total = item.Total;
                objDetail.ItemId = item.ItemId;
                // save back to ddatabase
                objRestaurantDBEntities.OrderDetails.Add(objDetail);
                objRestaurantDBEntities.SaveChanges();

                // Now come with transaction 
                Transaction objTrans = new Transaction();
                objTrans.ItemId = item.ItemId;
                objTrans.Quantity = item.Quantity;
                objTrans.TransactionDate = DateTime.Now;
                objTrans.TypeId = 2;
                objRestaurantDBEntities.Transactions.Add(objTrans);
                objRestaurantDBEntities.SaveChanges();


            }

            return true;
        }
    }
}