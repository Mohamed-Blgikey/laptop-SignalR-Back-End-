using App.BL.Interface;
using App.DAL.Entity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Laptop__.net_core_API__SignalR__.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationsController : ControllerBase
    {
        private readonly IBaseRep<Notification> rep;

        public NotificationsController(IBaseRep<Notification> rep)
        {
            this.rep = rep;
        }
        [HttpGet]
        [Route("~/GetNotificaions")]
        public async Task<IActionResult> GetAll()
        {
            var data =  await rep.GetAll();
            return Ok(data);
        }

    }
}
