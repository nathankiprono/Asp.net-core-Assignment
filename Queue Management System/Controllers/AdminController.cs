using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Queue_Management_System.Models;
using Queue_Management_System.QueueDbContext;

namespace Queue_Management_System.Controllers
{
   [Authorize]

    public class AdminController : Controller
    {
        private readonly QueueDbConnectionDbContext _context;
        public AdminController(QueueDbConnectionDbContext context)
        {

            _context = context;

        }
        public class AllData
        {
            public string xValue;
            public double yValue;
            public double Score;
            public string text;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            ViewBag.checkins = _context.Check_Ins.Where(k => k.Status == 3 && k.ischecked == true).Count();

            var waittime=_context.Check_Ins.Where(k =>  k.Status ==0).OrderBy(k=>k.CheckinTime).Select(K => K.CheckinTime).First();
            var waittime1 = _context.Check_Ins.Where(k => k.ischecked == true && k.Status == 1).Select(K => K.CheckinTime).First();

            var x = waittime;   
            var y = waittime1.Subtract(waittime);
            double z =Convert.ToInt32(y.TotalHours);

            double zz = Math.Round(z, 2);

            var waittime2 = _context.Check_Ins.Where(k => k.ischecked == true && k.Status == 1).Select(K => K.CheckinTime).First();
            var waittime3 = _context.Check_Ins.Where(k => k.ischecked == true && k.Status == 3).Select(K => K.CheckinTime).First();

            var x1 = waittime2;
            var y1 = waittime3.Subtract(waittime2);
            var z1 = y1.TotalHours;


            double zz1 = Convert.ToInt32(y1.TotalHours);

            double zzz1 = Math.Round(zz1, 2);


            List<AllData> chartData = new List<AllData>
            {

               new AllData {  xValue ="Average Waiting Time", yValue =zz, text = "Average Waiting Time:"+zz },
               new AllData { xValue = "Average Service Time ", yValue = zzz1  , text = "Average Service Time:"+zzz1  }

            };
            ViewBag.dataSource = chartData;


            return View();
        }
        [HttpGet]
        public IActionResult ServiceList()
        {
            General general = new General();


            general.services = _context.Services.ToList();
            general.serviceproviders = _context.Serviceproviders.ToList();


            return View(general);

        }
        [HttpGet]
        public IActionResult ServiceProvidersList()
        {
            return View(_context.Serviceproviders.ToList());
        }

        [HttpGet]
        public IActionResult ServiceProviders()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ServiceProviders(serviceproviders serviceProvider)
        {
            if (ModelState.IsValid)
            {
                _context.Add(serviceProvider);

                _context.SaveChanges();
                return RedirectToAction(nameof(ServiceProvidersList));
            }

            return View(serviceProvider);
        }
        [HttpGet]
        public IActionResult Service()
        {
            ViewBag.serviceprovider = new SelectList(_context.Serviceproviders, "Id", "Service_Providers");

            return View();
        }
        [HttpPost]
        public IActionResult Service(Services services)
        {
            ViewBag.serviceprovider = new SelectList(_context.Serviceproviders, "Id", "Service_Providers");

            if (ModelState.IsValid)
            {
                _context.Add(services);

                _context.SaveChanges();
                return RedirectToAction(nameof(ServiceList));
            }


            return View(services);
        }

    }
}
