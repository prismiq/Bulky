using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Bulky.Models;
using Bulky.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Bulky.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public OrderVM OrderVM { get; set; }


        public OrderController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index() 
        {
            return View();
        }

        public IActionResult Details(int orderId) 
        {

            OrderVM orderVM= new() {
                OrderHeader = _unitOfWork.OrderHeader.Get(u=>u.Id==orderId, includeProperties:"ApplicationUser"),
                OrderDetail = _unitOfWork.OrderDetail.GetAll(u => u.OrderHeaderId == orderId, includeProperties:"Product")
            };

            return View(orderVM);
        }

        [HttpGet]
        public IActionResult GetAll(string status) 
        {
            IEnumerable<OrderHeader> objOrderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();
            
            if (User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee)) {
                objOrderHeader = _unitOfWork.OrderHeader.GetAll(includeProperties:"ApplicationUser").ToList();
            } else {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

                objOrderHeader = _unitOfWork.OrderHeader.GetAll(u=>u.ApplicationUserId == userId, includeProperties:"ApplicationUser");
            }

            switch(status) {
                case "pending":
                    objOrderHeader = objOrderHeader.Where(u=> u.PaymentStatus == SD.PaymentStatusPending);
                    break;
                case "inprocess":
                    objOrderHeader = objOrderHeader.Where(u=> u.PaymentStatus == SD.StatusInProcess);
                    break;
                case "completed":
                    objOrderHeader = objOrderHeader.Where(u=> u.PaymentStatus == SD.PaymentStatusPending);
                    break;
                case "approved":
                    objOrderHeader = objOrderHeader.Where(u=> u.PaymentStatus == SD.PaymentStatusApproved);
                    break;
             }
            
            return Json(new { data = objOrderHeader });
        }

        [HttpPost]
        public IActionResult UpdateOrderDetail(int orderId) {

            var orderHeaderFromDb = _unitOfWork.OrderHeader.Get(u => u.Id == OrderVM.OrderHeader.Id);
            orderHeaderFromDb.Name = OrderVM.OrderHeader.Name;
            orderHeaderFromDb.PhoneNumber = OrderVM.OrderHeader.PhoneNumber;
            orderHeaderFromDb.StreetAddress = OrderVM.OrderHeader.StreetAddress;
            orderHeaderFromDb.City = OrderVM.OrderHeader.City;
            orderHeaderFromDb.State = OrderVM.OrderHeader.State;
            orderHeaderFromDb.PostalCode = OrderVM.OrderHeader.PostalCode;
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.Carrier)) {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.Carrier;
            }
            if (!string.IsNullOrEmpty(OrderVM.OrderHeader.TrackingNumber)) {
                orderHeaderFromDb.Carrier = OrderVM.OrderHeader.TrackingNumber;
            }

            //Update DB & Save
            _unitOfWork.OrderHeader.Update(orderHeaderFromDb);
            _unitOfWork.Save();

            TempData["Success"] = "Order Details Updated Successfully.";

            return RedirectToAction(nameof(Details), new {orderId= orderHeaderFromDb.Id});    
        }

        [HttpPost]
        public IActionResult StartProcessing() {
            _unitOfWork.OrderHeader.UpdateStatus(OrderVM.OrderHeader.Id, SD.StatusInProcess); 
            _unitOfWork.Save();
            TempData["success"] = "Order Details Updated";

            return RedirectToAction(nameof(Details), new {orderId = OrderVM.OrderHeader.Id});
        }
    }
}
