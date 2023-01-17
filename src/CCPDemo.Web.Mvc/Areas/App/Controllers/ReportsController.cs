using Abp.AspNetCore.Mvc.Authorization;
using CCPDemo.Authorization;
using CCPDemo.Web.Areas.App.Models.Risks;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using System.Collections.Generic;
using CCPDemo.Risks;
using CCPDemo.Risks.Dtos;
using CCPDemo.RiskRatings.Dtos;

namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Risks)]
    public class ReportsController : CCPDemoControllerBase
    {
        private readonly IRisksAppService _risksAppService;

        public ReportsController(IRisksAppService risksAppService)
        {
            _risksAppService = risksAppService;
        }

        public IActionResult Index()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult OverdueRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult OnGoingRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public IActionResult ClosedRisks()
        {
            var model = new RisksViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public async Task<IActionResult> FilteredRisks()
        {

            GetRiskForEditOutput getRiskForEditOutput;

            var model = new RisksViewModel
            {
                FilterText = Request.Query["filterText"],
                RiskTypeList = await _risksAppService.GetAllRiskTypeForTableDropdown(),
                RiskOrganizationUnitList = await _risksAppService.GetAllOrganizationUnitForTableDropdown(),
                RiskStatusList = await _risksAppService.GetAllStatusForTableDropdown(),
                RiskRiskRatingList = await _risksAppService.GetAllRiskRatingForTableDropdown()
            };


            return View(model);
        }

        public IActionResult SendMail()
        {
            string fromEmail = "wilf.funsho@gmail.com";
            MailMessage mailMessage = new MailMessage(fromEmail, "teddywilf91@gmail.com", "Risk Managment", "Body");
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential(fromEmail, "password");
            try
            {
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //Error
                //Console.WriteLine(ex.Message);
                return Json(ex.Message);
            }

            return Json("Mail Sent");


        }
    }
}
