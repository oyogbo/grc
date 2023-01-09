using CCPDemo.Authorization.Users;
using CCPDemo.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCPDemo.KRIComents
{
    public class KRICommentService : CCPDemoAppServiceBase, IKRICommentService
    {
        private readonly IKRICommentRepo _kRICommentRepo;

        public KRICommentService(IKRICommentRepo kRICommentRepo)
        {
            _kRICommentRepo = kRICommentRepo;
        }

        public bool AddComment(Comment comment)
        {
            User user = GetCurrentUser();
            if (user != null)
            {
                comment.UserId = user.FullName + " " + user.EmailAddress;
            }

            _kRICommentRepo.Insert(comment);
            return true;
        }

        public bool AddComment(string comment, int KRIId)
        {
            Comment commentToAdd = new Comment();
            User user = GetCurrentUser();
            if (user != null)
            {
                commentToAdd.UserId = user.FullName + " " + user.EmailAddress;
            }
            commentToAdd.CommentText = comment;
            commentToAdd.DateCreated = DateTime.Now.ToString();
            commentToAdd.KRIId = KRIId;
            _kRICommentRepo.Insert(commentToAdd);
            return true;
        }
    }
}
