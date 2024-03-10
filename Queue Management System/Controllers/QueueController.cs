using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Queue_Management_System.Models;
using Queue_Management_System.QueueDbContext;
using System;
using static Queue_Management_System.Models.Check_In;

namespace Queue_Management_System.Controllers
{
    public class QueueController : Controller
    {
        private readonly QueueDbConnectionDbContext _context;
        public QueueController(QueueDbConnectionDbContext context)
        {
          

            _context = context;

        }

     
      

        [HttpPost]
        public ActionResult Save(List<Check_In> check_Ins)
        {

          
            foreach (Check_In check_In in check_Ins)
            {
                Check_In? updatedcheckins = _context.Check_Ins.ToList().Find(p => p.Id== check_In.Id);
                updatedcheckins.ischecked = check_In.ischecked;
                if (updatedcheckins.ischecked = check_In.ischecked)
                {
                    var UserId = HttpContext.Session.GetString("UserId");
                    DateTime dateTime = DateTime.UtcNow;
               
                    var newdata = new Check_In
                    {
                      
                        CustomerName = updatedcheckins.CustomerName,
                        description = updatedcheckins.description,
                        TicketNo = updatedcheckins.TicketNo,
                        Servicetypes = updatedcheckins.Servicetypes,
                        Status = 1,
                        UserId= UserId,
                        ischecked = true,
                        CheckinTime = dateTime,


                       };
                        _context.Add(newdata);
                        _context.SaveChanges();
                  
                  
                }
                return RedirectToAction("ServicePoint");

            }
            return View();
            
        }

        public IActionResult Index()
        {

            General general = new General();


            general.services = _context.Services.ToList();
            general.serviceproviders = _context.Serviceproviders.ToList();
            general.check_Ins = _context.Check_Ins.Where(k => k.ischecked == true&&k.Status==1).OrderBy(k => k.CheckinTime).ToList();
            return View(general);
        }

        [HttpGet]
        public IActionResult CheckinList()
        {

            General general = new General();


            general.services = _context.Services.ToList();
            general.serviceproviders = _context.Serviceproviders.ToList();
            general.check_Ins = _context.Check_Ins.Where(k => k.TicketNo == HttpContext.Session.GetString("Ticket")).ToList();

            return View(general);

        }
        [HttpGet]
        public IActionResult CheckinPage()
        {

            var servicename = new SelectList(_context.Services, "Id", "ServiceName");
            ViewBag.servicename = servicename;
            return View();

        }
        [HttpPost]
        public IActionResult CheckinPage(Check_In check_In)
        {
            var servicename = new SelectList(_context.Services, "Id", "ServiceName");
            ViewBag.servicename = servicename;
            Random r = new Random();
            var x = r.Next(000);
            var y = r.Next(10, 99);
            var ticket = "#" + x + "-0" + y;
            HttpContext.Session.SetString("Ticket", ticket);
            check_In.TicketNo = ticket;

            check_In.Status = 0;
            check_In.ischecked = false;

            _context.Add(check_In);

            _context.SaveChanges();
            return RedirectToAction(nameof(CheckinList));



        }



        [HttpGet]
        public IActionResult WaitingPage()
        {
            General general = new General();


            general.services = _context.Services.ToList();
            general.serviceproviders = _context.Serviceproviders.ToList();
            general.check_Ins =  _context.Check_Ins.Where(k=>k.ischecked==false&&k.Status==0).ToList();
           

            return View(general);

        }



        [Authorize, HttpGet]
        public IActionResult ServicePoint()
        {
            var UserId = HttpContext.Session.GetString("UserId");
            General general = new General();


            general.services = _context.Services.ToList();
            general.serviceproviders = _context.Serviceproviders.ToList();
            general.check_Ins = _context.Check_Ins.Where(k => k.ischecked == true && k.Status == 1&&k.UserId== UserId).OrderBy(k => k.CheckinTime).ToList();
            return View(general);

        }
        [HttpGet]
        public IActionResult Details(int? id)
        {

            var checkindetails = _context.Check_Ins
                .FirstOrDefault(m => m.Id == id);

            ViewBag.id = checkindetails.Id;
            return View(checkindetails);
        }
        [HttpPost]

        public IActionResult Finished(int? id)
        {
            var checkins = _context.Check_Ins.FirstOrDefault(k => k.Id == id);
            DateTime dateTime = DateTime.UtcNow;

            if (checkins != null)
            {
               
                var newdata1 = new Check_In
                {

                    CustomerName = checkins.CustomerName,
                    description = checkins.description,
                    TicketNo = checkins.TicketNo,
                    Servicetypes = checkins.Servicetypes,
                    Status = 3,
                    ischecked = checkins.ischecked,
                    CheckinTime = dateTime,
                };
                _context.Add(newdata1);
                _context.SaveChanges();

             

            }
            return RedirectToAction(nameof(ServicePoint));
        }
        [HttpPost]

        public IActionResult Noshow(int? id)
        {
            var checkins = _context.Check_Ins.FirstOrDefault(k => k.Id == id);

            DateTime dateTime = DateTime.UtcNow;
            if (checkins != null)
            {


                var newdata2 = new Check_In
                {

                    CustomerName = checkins.CustomerName,
                    description = checkins.description,
                    TicketNo = checkins.TicketNo,
                    Servicetypes = checkins.Servicetypes,
                    Status = 2,
                    ischecked = checkins.ischecked,
                    CheckinTime = dateTime,
                };
                _context.Add(newdata2);
                _context.SaveChanges();

            

            }
            return RedirectToAction(nameof(ServicePoint));
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var checkins = _context.Check_Ins.Find(id);
            _context.Check_Ins.Remove(checkins);
            _context.SaveChanges();
            return RedirectToAction(nameof(ServicePoint));
        }


    }
}
