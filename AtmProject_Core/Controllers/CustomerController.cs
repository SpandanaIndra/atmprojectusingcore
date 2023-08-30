using AtmProject_Core.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.NetworkInformation;
using System.Security.Cryptography;

namespace AtmProject_Core.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AccountsDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        public CustomerController(AccountsDbContext context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;

        }

        //Register Action Method
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Accounts cust)
        {
            if (ModelState.IsValid)
            {
                _context.Accounts.Add(cust);
               int res= _context.SaveChanges();

                if(res!=0)
                {
                    ViewBag.msg = "Account Created Successfully";
                }
            }
                return View(cust);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(Accounts a)
        {
           
            var cust=   _context.Accounts.FirstOrDefault(c=>c.AccountsId==a.AccountsId&&c.Pin==a.Pin);

            if (cust == null)
            {
                ViewBag.msg = "InValid User";
            }
            else
            {
                TempData["name"]=cust.CustomerName;
                //  TempData.Keep("name");
                _contextAccessor.HttpContext.Session.SetInt32("accno", cust.AccountsId);
                _contextAccessor.HttpContext.Session.SetString("pin", cust.Pin);
                string bal = cust.Balance.ToString();
                _contextAccessor.HttpContext.Session.SetString("bal", bal);



                return RedirectToAction("Transactions");
            }
            

            return View();
        }
        public IActionResult Transactions()
        {
       ViewBag.name = TempData.Peek("name") as string;
         

            return View();
        }
        [HttpGet]
        public IActionResult Deposit()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Deposit(Accounts a)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {


                    var accno = _contextAccessor.HttpContext.Session.GetInt32("accno");
                    var pin = _contextAccessor.HttpContext.Session.GetString("pin");


                    //var bal = _contextAccessor.HttpContext.Session.GetString("bal");
                    //double balance = Convert.ToDouble(bal);
                    //  a.AccountsId = accno.Value;
                    // a.Pin = pin;


                    var customer = _context.Accounts.FirstOrDefault(c => c.AccountsId == accno && c.Pin == pin);

                    var NewBalance = customer.Balance + a.Balance;

                    if (customer != null)
                    {
                        customer.Balance = NewBalance;
                        var res = _context.SaveChanges();
                       
                        if (res != 0)
                        {
                           transaction.Commit();
                            ViewBag.msg = "Transaction completed Successfully..!!";
                            ViewBag.bal = NewBalance;
                           
                        }
                    }
                }
                catch (Exception)
                {
                    ViewBag.msg = "Transaction Failed..!!";

                    // An error occurred, so roll back the transaction
                    transaction.Rollback();
                    // Handle the error, e.g., log or display an error message
                    return View("Error");
                }
            }




                return View();
        }
        public IActionResult WithDraw()
        {

            return View();
        }
        [HttpPost]
        public IActionResult WithDraw(Accounts a)
        {

           
                using (var transaction =  _context.Database.BeginTransaction())
                {
                    try
                    {
                        var accno = _contextAccessor.HttpContext.Session.GetInt32("accno");
                        var pin = _contextAccessor.HttpContext.Session.GetString("pin");



                        var customer = _context.Accounts.FirstOrDefault(c => c.AccountsId == accno && c.Pin == pin);

                        if (customer.Balance > a.Balance)
                        {

                            var NewBalance = customer.Balance - a.Balance;

                            if (customer != null)
                            {
                                customer.Balance = NewBalance;
                                var res = _context.SaveChanges();
                                if (res != 0)
                                {
                                    ViewBag.msg = "Transaction completed Successfully..!!";
                                    ViewBag.bal = NewBalance;
                                 transaction.Commit();
                            }
                            }


                        }
                        else
                            ViewBag.res = "Insufficient Funds..!!";

                    }
                    catch (Exception)
                    {
                        // An error occurred, so roll back the transaction
                         transaction.Rollback();
                        // Handle the error, e.g., log or display an error message
                        return View("Error");
                    }

                    return View();
                
                }
        }

        public IActionResult CurrentBalance()
        {
            var accno = _contextAccessor.HttpContext.Session.GetInt32("accno");
            var pin = _contextAccessor.HttpContext.Session.GetString("pin");



            var customer = _context.Accounts.FirstOrDefault(c => c.AccountsId == accno && c.Pin == pin);
            ViewBag.bal = customer.Balance;

            return View();
        }
        public IActionResult ChangePin()
        {

            return View();
        }
        [HttpPost]
        public IActionResult ChangePin(Accounts a,string newpin)
        {
            var customer = _context.Accounts.FirstOrDefault(c => c.AccountsId == a.AccountsId && c.Pin == a.Pin);
            if (customer != null)
            {
                customer.Pin = newpin;
                var res = _context.SaveChanges();
                if (res != 0)
                {
                    ViewBag.msg = "Pin Changed Successfully..!!";
                  
                }
                else
                    ViewBag.msg = "Pin not Changed ..!!";

            }
            return View();
        }
        public IActionResult MiniStatement()
        {

            var accno = _contextAccessor.HttpContext.Session.GetInt32("accno");
            var query = $"EXEC proc_ministmt @p0";
            //var res1 =_context.Database.ExecuteSqlRaw(query, accno);
            var ministmt = _context.Set<Transactions>().FromSqlRaw(query, accno).ToList();

            return View(ministmt);
        }
    }
}
