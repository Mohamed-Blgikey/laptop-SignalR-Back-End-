using App.BL.Helper;
using App.BL.Interface;
using App.BL.Model;
using App.DAL.Entity;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Laptop__.net_core_API__SignalR__.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LaptopsController : ControllerBase
    {
        private readonly IBaseRep<Laptop> rep;
        private readonly IBaseRep<Notification> repNotfy;
        private readonly IHubContext<SignalHub> hub;
        private List<string> allaowExtentions = new List<string> { ".jpg",".png"};
        private long allaowSize = 1048576;
        public LaptopsController(IBaseRep<Laptop> rep,IBaseRep<Notification> repNotfy,IHubContext<SignalHub> hub)
        {
            this.rep = rep;
            this.repNotfy = repNotfy;
            this.hub = hub;
        }
        [HttpGet]
        [Route("~/GetLaptops")]
        public async Task<IActionResult> GetPostsAsync()
        {
            var data = await rep.GetAll();
            return Ok(data);
        }


        [HttpPost]
        [Route("~/AddLaptop")]
        public async Task<IActionResult> AddLaptop([FromBody] LaptopDTO dTO)
        {
          
            var model = new Laptop
            {
                Id = dTO.Id,
                Name = dTO.Name,
                Ram = dTO.Ram,
                Storage = dTO.Storage,
                Poster = dTO.Poster
            };

            var data = await rep.Add(model);

            if (data != null)
            {
                Notification notification = new Notification
                {
                    Id = 0,
                    Title = $"Laptop {data.Name} was Added"
                };
                await repNotfy.Add(notification);
                this.hub.Clients.All.SendAsync("LaptopAction");
            }
            return Ok(data);
        }
        
        
        [HttpPut]
        [Route("~/EditLaptop")]
        public async Task<IActionResult> EditLaptop([FromBody] LaptopDTO dTO)
        {
            var lap = await rep.GetById(dTO.Id);
            if (lap == null)
                return Ok(new { message = "LaptopNotFound" });

            lap.Poster = dTO.Poster;
            lap.Name = dTO.Name;
            lap.Ram = dTO.Ram;
            lap.Storage = dTO.Storage;
          

            var data = await rep.Edit(lap);

            if (data != null)
            {
                Notification notification = new Notification
                {
                    Id = 0,
                    Title = $"Laptop {data.Name} was Edited"
                };
                await repNotfy.Add(notification);
                this.hub.Clients.All.SendAsync("LaptopAction");
            }
            return Ok(data);
        }


        [HttpPost]
        [Route("~/DeleteLaptop")]
        public async Task<IActionResult> DeleteLaptop([FromBody] int id)
        {
           
            var model = await rep.GetById(id);
            var data = await rep.Delete(model);

            if (data != null)
            {
                Notification notification = new Notification
                {
                    Id = 0,
                    Title = $"Laptop {data.Name} was Deleted"
                };
                await repNotfy.Add(notification);
                this.hub.Clients.All.SendAsync("LaptopAction");
            }
            return Ok(data);
        }


        [Route("~/SavePhoto")]
        [HttpPost]
        public async Task<IActionResult> SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = Guid.NewGuid() + postedFile.FileName;
                var physicalPath = Directory.GetCurrentDirectory() + "/wwwroot/Img/" + filename;

                using (var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    await postedFile.CopyToAsync(stream);
                }

                return Ok(new { message = filename });
            }
            catch (Exception)
            {
                return Ok(new { message = "Error !" });
            }
        }

        [HttpPost]
        [Route("~/UnSavePhoto")]
        public JsonResult UnSaveFile([FromBody] PhotoDTO photoVM)
        {
            try
            {
                if (System.IO.File.Exists(Directory.GetCurrentDirectory() + "/wwwroot/Img/" + photoVM.Name))
                {
                    System.IO.File.Delete(Directory.GetCurrentDirectory() + "/wwwroot/Img/" + photoVM.Name);
                }

                return new JsonResult(new { message = "Deleted !" });
            }
            catch (Exception)
            {

                return new JsonResult("Error!");
            }
        }

    }
}
