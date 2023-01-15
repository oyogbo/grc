using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using AutoMapper;
using CCPDemo.Authorization;
using CCPDemo.Authorization.Roles;
using CCPDemo.KeyRiskIndicatorHistories;
using CCPDemo.KeyRiskIndicators;
using CCPDemo.KeyRiskIndicators.Dtos;
using CCPDemo.KeyRiskIndicators.Service.Interface;
using CCPDemo.Web.Areas.App.Models.Error;
using CCPDemo.Web.Areas.App.Models.KeyRiskIndicators;
using CCPDemo.Web.Areas.App.Models.UploadKeyRiskIndicators;
using CCPDemo.Web.Controllers;
using CsvHelper;
using GraphQL.NewtonsoftJson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Org.BouncyCastle.Crypto.Digests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


namespace CCPDemo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class KeyRiskIndicatorsController : CCPDemoControllerBase
    {
        private readonly IKeyRiskIndicatorsAppService _keyRiskIndicatorsAppService;
        private readonly IKeyRiskIndicatorService _keyRiskIndicatorHistoryService;
        private readonly IKRIService _kRIService;
        private readonly ISerialiserService _serialiserService;
        private IHostEnvironment Environment;
        private IConfiguration Configuration;
        private readonly IRepository<Role> _roleRepository;

        private IMapper _mapper;

        private readonly IRepository<KeyRiskIndicator> _keyRiskIndicatorRepository;




        public KeyRiskIndicatorsController(IKeyRiskIndicatorsAppService keyRiskIndicatorsAppService,
                                           IMapper mapper, ISerialiserService serialiserService,
                                           IHostEnvironment _environment,
                                           IKeyRiskIndicatorService keyRiskIndicatorHistoryService,
                                           IKRIService kRIService,
                                            IRepository<Role> roleRepository,
                                           IRepository<KeyRiskIndicator> keyRiskIndicatorRepository,
                                           IConfiguration _configuration)
        {
            _keyRiskIndicatorsAppService = keyRiskIndicatorsAppService;
            _kRIService = kRIService;
            _serialiserService = serialiserService;
            Environment = _environment;
            Configuration = _configuration;
            _mapper = mapper;
            _keyRiskIndicatorHistoryService = keyRiskIndicatorHistoryService;
            _keyRiskIndicatorRepository = keyRiskIndicatorRepository;
            _roleRepository= roleRepository;    
        }

        public async Task<ActionResult> Index(string ReferenceId)
        {

            var riskIndicators = await _keyRiskIndicatorsAppService.GetAllByRefId(ReferenceId);
            KeyRiskIndicatorViewModel model = new KeyRiskIndicatorViewModel();
            var userRoles =  await _keyRiskIndicatorHistoryService.GetCurrentUserRoles();

            List<string> roles = new List<string>();

            var rolesfromdb = _roleRepository.GetAll();
            foreach (var item in userRoles)
            {
                roles.Add(rolesfromdb.FirstOrDefault(x => x.ConcurrencyStamp == item || x.Name == item).DisplayName);

            }

            if (roles.Contains("ERM"))
            {
                model.IsERM = true;
            }

            if (roles.Contains("Admin"))
            {
                model.IsAdmin = true;
            }

            model.KeyRiskIndicators = riskIndicators.KeyRiskIndicators;
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators_Create, AppPermissions.Pages_KeyRiskIndicators_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetKeyRiskIndicatorForEditOutput getKeyRiskIndicatorForEditOutput;

            if (id.HasValue)
            {
                getKeyRiskIndicatorForEditOutput = await _keyRiskIndicatorsAppService.GetKeyRiskIndicatorForEdit(new EntityDto { Id = (int)id });
            }
            else
            {
                getKeyRiskIndicatorForEditOutput = new GetKeyRiskIndicatorForEditOutput
                {
                    KeyRiskIndicator = new CreateOrEditKeyRiskIndicatorDto()
                };
            }

            var viewModel = new CreateOrEditKeyRiskIndicatorModalViewModel()
            {
                KeyRiskIndicator = getKeyRiskIndicatorForEditOutput.KeyRiskIndicator,

            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> ViewKeyRiskIndicatorModal(int id)
        {
            var getKeyRiskIndicatorForViewDto = await _keyRiskIndicatorsAppService.GetKeyRiskIndicatorForView(id);

            var model = new KeyRiskIndicatorViewModel()
            {
                KeyRiskIndicator = getKeyRiskIndicatorForViewDto.KeyRiskIndicator
            };
            return PartialView("_ViewKeyRiskIndicatorModal", model);
        }

        [HttpPost]
        [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators_Create, AppPermissions.Pages_KeyRiskIndicators_Edit)]
        public async Task<JsonResult> UploadKRI()
        {
            try
            {
                var csvFile = Request.Form.Files.First();

                //Check input
                if (csvFile == null)
                {
                    throw new Abp.UI.UserFriendlyException(L("File_Empty_Error"));
                }

                if (csvFile.Length > 5120000) //5MB
                {
                    throw new UserFriendlyException(L("CSVFile_SizeLimit_Error"));
                }

                var keyRiskIndicatorsData = _keyRiskIndicatorsAppService.ReadCSV<KeyRiskIndicatorUploadDto>(csvFile.OpenReadStream());

                DataTable dt = new DataTable("KeyRiskIndicators");

                return Json(new AjaxResponse(keyRiskIndicatorsData));
            }
            catch (UserFriendlyException ex)
            {
                return Json(new AjaxResponse(new ErrorInfo(ex.Message)));
            }

        }

        public async Task<IActionResult> Upload()
        {
            IFormFile file = Request.Form.Files[0];

            using (Stream stream = file.OpenReadStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    //var records = csv.GetRecords<KeyRiskIndicator>();

                    DataTable dt = new DataTable("KeyRiskIndicators");

                    while (csv.Read())
                    {
                        var row = dt.NewRow();
                        foreach (DataColumn column in dt.Columns)
                        {
                            row[column.ColumnName] = csv.GetField(column.DataType, column.ColumnName);
                        }
                        dt.Rows.Add(row);
                    }
                }

                string data = await reader.ReadToEndAsync();
                // Do something with file data
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ReadCSV(AddUploadKRIDto addUploadKRI)
        {

           long orgIg = await _keyRiskIndicatorHistoryService.GetUserOrganisationDepartmentId();
            if (orgIg == 0)
            {
                ErrorView errorView = new ErrorView();
                errorView.Message = "You have to belong to a department to uppload a Key risk indicator";
                errorView.BackController = "UploadKeyRiskIndicator";
                errorView.BackAction = "Index";
                return RedirectToAction("Index", "Error", errorView, fragment: null);
            }

            IFormFile fileToRead = Request.Form.Files[0];
            string  nameForCheck = fileToRead.FileName;

            if (!nameForCheck.EndsWith(".xlsx"))
            {
                ErrorView errorView = new ErrorView();
                errorView.Message = "File type is invalid";
                errorView.BackController = "UploadKeyRiskIndicator";
                errorView.BackAction = "Index";
                return RedirectToAction("Index", "Error",  errorView, fragment: null);
            }


            string filePath = "";
            if (fileToRead != null)
            {
                string path = Path.Combine(this.Environment.ContentRootPath, "Uploads");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                string fileName = Path.GetFileName(fileToRead.FileName);
                filePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    fileToRead.CopyTo(stream);
                }
            }
            var response = await _serialiserService.ReadFileAsync(filePath);

            string ReferenceId = "COR/HRM/" + DateTime.Now.Year.ToString() + Guid.NewGuid().ToString().Substring(0, 4);

            foreach (var item in response)
            {
                CreateOrEditKeyRiskIndicatorDto dataToAdd = new CreateOrEditKeyRiskIndicatorDto();

                dataToAdd.ReferenceId = ReferenceId;
                dataToAdd.Status = "Not Approved";
                dataToAdd.Activity = item.Activity;
                dataToAdd.BusinessLines = item.BussinessLines;
                dataToAdd.PotentialRisk = item.PotentailRisk;
                dataToAdd.LikelihoodOfOccurrence_rrr = item.LikelihoodOfOccurance_rrr;
                dataToAdd.LikelihoodOfImpact_rrr = item.LikelihoodOfImpact_rrr;
                dataToAdd.LikelihoodOfImpact_irr = item.LikelihoodOfImpact_irr;
                dataToAdd.LikelihoodOfOccurrence_irr = item.LikelihoodOfOccurance_irr;
                dataToAdd.MitigationPlan = item.MitigationPlan;
                dataToAdd.SubProcess = item.SubProcess;
                dataToAdd.Process = item.Proccess;
                dataToAdd.ControlEffectiveness = item.ControlOfEffectiveness;
                dataToAdd.IsControlInUse = item.IsControlInUse;
                dataToAdd.KeyControl = item.KeyControl;

              var rex =   _keyRiskIndicatorsAppService.CreateOrEdit(dataToAdd);

            }

            var res = await _kRIService.SendAddKRIEmailNotification();

            KRIHistoryAddDTO historyToAdd = new KRIHistoryAddDTO();
            historyToAdd.Status = "Not Approved";
            historyToAdd.BussinessLine = response[0].BussinessLines;
            historyToAdd.TotalRecord = response.Count().ToString();
            historyToAdd.Department = ReferenceId.Substring(0, 7);
            historyToAdd.ReferenceId = ReferenceId;
            historyToAdd.OrganizationUnit = addUploadKRI.OrganizationUnit;

            var resx = _keyRiskIndicatorHistoryService.AddKeyIndicatorHistory(historyToAdd);

            return RedirectToAction("Index", "KeyRiskIndicatorHistory");
        }

        public class CsvUploadModel
        {
            public IFormFile file { get; set; }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllKeyRiskIndicators()
        {
            GetAllKeyRiskIndicatorsInput getAllKeyRiskIndicatorsInput = new GetAllKeyRiskIndicatorsInput();
            var riskIndicators = await _keyRiskIndicatorsAppService.GetAll(getAllKeyRiskIndicatorsInput);
            List<KeyRiskIndicator> listToReturn = new List<KeyRiskIndicator>();
            foreach (var risk in riskIndicators.Items)
            {

                KeyRiskIndicator dataToAdd = new KeyRiskIndicator();

                dataToAdd.ReferenceId = "COR/HRM/" + DateTime.Now.Year.ToString() + Guid.NewGuid().ToString().Substring(0, 4);
                dataToAdd.Status = "pending";
                dataToAdd.Activity = risk.KeyRiskIndicator.Activity;
                dataToAdd.BusinessLines = risk.KeyRiskIndicator.BusinessLines;
                dataToAdd.PotentialRisk = risk.KeyRiskIndicator.PotentialRisk;
                dataToAdd.LikelihoodOfOccurrence_rrr = risk.KeyRiskIndicator.LikelihoodOfOccurrence_rrr;
                dataToAdd.LikelihoodOfImpact_rrr = risk.KeyRiskIndicator.LikelihoodOfImpact_rrr;
                dataToAdd.LikelihoodOfImpact_irr = risk.KeyRiskIndicator.LikelihoodOfImpact_irr;
                dataToAdd.LikelihoodOfOccurrence_irr = risk.KeyRiskIndicator.LikelihoodOfOccurrence_irr;
                dataToAdd.MitigationPlan = risk.KeyRiskIndicator.MitigationPlan;
                dataToAdd.SubProcess = risk.KeyRiskIndicator.SubProcess;
                dataToAdd.Process = risk.KeyRiskIndicator.Process;
                dataToAdd.ControlEffectiveness = risk.KeyRiskIndicator.ControlEffectiveness;
                dataToAdd.IsControlInUse = risk.KeyRiskIndicator.IsControlInUse;
                dataToAdd.KeyControl = risk.KeyRiskIndicator.KeyControl;

                listToReturn.Add(dataToAdd);
            }

            return View("Index");

        }

        [HttpPatch()]
        public async Task<bool> ApproveKRI([FromBody] List<int> newId)
        {
            bool response = await _keyRiskIndicatorsAppService.ApproveclineKRI(newId);
            if (response)
            {
                var KRI = _keyRiskIndicatorRepository.Get(newId[0]);
                var uploaderEmail = await _keyRiskIndicatorHistoryService.GetKRIUploaderEmail(KRI.ReferenceId);
                List<string> listToReturn = new List<string>();
                listToReturn.Add(uploaderEmail);
                _kRIService.ChangeKRIStatusEmailNotificationAsync(listToReturn, KRI.ReferenceId, KRI.Status);
            }
            return false;
        }


        [HttpPost()]
        public IActionResult EditKRI(EditKRI model)
        {
            KeyRiskIndicator dataToInsert = _keyRiskIndicatorRepository.Get(model.Id);

            if (dataToInsert != null)
            {
                dataToInsert.Activity = model.Activity;
                dataToInsert.MitigationPlan = model.MitigationPlan;
                dataToInsert.PotentialRisk = model.PotentialRisk;
                dataToInsert.LikelihoodOfOccurrence_rrr = model.LikelihoodOfOccurrence_rrr;
                dataToInsert.LikelihoodOfOccurrence_irr = model.LikelihoodOfOccurrence_irr;
                dataToInsert.LikelihoodOfImpact_irr = model.LikelihoodOfImpact_irr;
                dataToInsert.LikelihoodOfImpact_rrr =   model.LikelihoodOfImpact_rrr;
                dataToInsert.ControlEffectiveness = model.ControlEffectiveness;
                dataToInsert.KeyControl = model.KeyControl;
                dataToInsert.SubProcess = model.SubProcess;
                dataToInsert.Process = model.Process;
                dataToInsert.IsControlInUse = model.IsControlInUse; 
            }
            _keyRiskIndicatorRepository.Update(dataToInsert);
            return RedirectToAction("ViewKRIDetails", "KeyRiskIndicators", new { KRIToViewId = model.Id }, fragment: null);
        }

        public IActionResult ViewKRI(int KRIToViewId)
        {
           var kriToView =  _keyRiskIndicatorRepository.Get(KRIToViewId);

            return View(kriToView);
        }


        public IActionResult ViewKRIDetails(int KRIToViewId)
        {
            var kriToView = _keyRiskIndicatorRepository.Get(KRIToViewId);
            return View(kriToView);
        }



        public async Task<IActionResult> DelineKRI(int Id)
        {
            bool response = await _keyRiskIndicatorsAppService.DeclineKRI(Id);
            return RedirectToAction("Index", "Comment");

        }

        public async Task<IActionResult> EditKRI(int Id)
        {
            bool response = await _keyRiskIndicatorsAppService.DeclineKRI(Id);
            return RedirectToAction("Index", "Comment");

        }

    }

}
