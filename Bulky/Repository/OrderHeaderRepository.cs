using Bulky.Data;
using Bulky.Models;
using Bulky.Repository;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class OrderHeaderRepository : Repository<OrderHeader>, IOrderHeaderRepository {
        private ApplicationDbContext _db;
        public OrderHeaderRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(OrderHeader obj)
        {
            _db.OrderHeaders.Update(obj);
        }

        public void UpdateStatus(int Id, string orderStatus, string? paymentStatus = null) {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == Id);
            if (orderFromDb != null) {
                orderFromDb.OrderStatus = orderStatus;
                if (!string.IsNullOrEmpty(paymentStatus)) {
                    orderFromDb.PaymentStatus = paymentStatus;
                }
            }
        }
        public void UpdateStripePaymentID(int Id, string SessionId, string paymentIntentId) {
            var orderFromDb = _db.OrderHeaders.FirstOrDefault(u => u.Id == Id);
            if (!string.IsNullOrEmpty(SessionId)) {
                orderFromDb.SessionId = SessionId;
            }
        }
	}
}
