using Abp.AspNetCore.Mvc.Authorization;
using CCPDemo.Authorization;
using CCPDemo.KRIComents;
using CCPDemo.Web.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace CCPDemo.Web.Areas.App.Controllers
{

    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_KeyRiskIndicators)]
    public class CommentController : CCPDemoControllerBase
    {
        private readonly IKRICommentRepo _commentRepo;
        private readonly IKRICommentService _commentService;
        public CommentController(IKRICommentRepo commentRepo, IKRICommentService commentService)
        {
            _commentRepo = commentRepo;
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<bool> Index([FromBody]AddCommenDto addCommenDto)
        {
           bool response =  _commentService.AddComment(addCommenDto.Comment, addCommenDto.KRIId);
            if (response)
            {
                return response;
            }
            return false;
        }

        [HttpGet]
        public JsonResult GetAllCommentByKRIID(int Id)
        {
           var commentToReturn =  _commentRepo.GetAllList().Where(comment => comment.KRIId == Id);
            return Json(commentToReturn);
        }
    }
}
