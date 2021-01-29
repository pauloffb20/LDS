using System;
using System.Data.Entity.Core;
using System.Linq;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using REBOOK.Data;
using REBOOK.Exceptions;
using REBOOK.Models;

namespace REBOOK.Controllers
{
    [Route("reports")]
    [ApiController]
    public class ReportController : Controller
    {
        private readonly ReportRepo _reportRepo;

        public ReportController()
        {
            _reportRepo = new ReportRepo();
        }
        
        /// <summary>
        /// Este método permite adicionar um report
        /// </summary>
        /// <param name="newReport"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult NewReport([FromBody] Report newReport)
        {
            try
            {
                _reportRepo.SubmitReport(newReport);
                return Json(newReport);
            }
            catch (ObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public JsonResult GetReports()
        {
            try
            {
                return Json(_reportRepo.GetAllReports());
            }
            catch (DatabaseIsEmptyException e)
            {
                return Json("No reports in database");
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public JsonResult GetReportByUserId(int id)
        {
            return Json(_reportRepo.GetReportByUserId(id).ToList());
        }
    }
}