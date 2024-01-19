using System.Collections.Generic;
using System.Linq;
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
        public OrderVM orderVM {get; set;}

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
            
            return View(orderVM);        
        }
    }
}
