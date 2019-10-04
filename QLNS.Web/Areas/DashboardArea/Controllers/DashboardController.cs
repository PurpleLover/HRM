using QLNS.Web.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackExchange.Redis;


namespace QLNS.Web.Areas.DashboardArea.Controllers
{
    public class DashboardController : BaseController
    {
        IConnectionMultiplexer _connectionMultiplexer;
        IDatabase _cacheDatabase;
        public DashboardController(IConnectionMultiplexer connectionMultiplexer,
            IDatabase cacheDatabase)
        {
            _connectionMultiplexer = connectionMultiplexer;
            _cacheDatabase = cacheDatabase;
        }
        // GET: DashboardArea/Dashboard
        [Authorize]
        public ActionResult Index()
        {
            //IDatabase db = _connectionMultiplexer.GetDatabase(10);
            _cacheDatabase.StringSet("hello", 123);
            var a = _cacheDatabase.StringGet("hello");


            return View();
        }
    }
}