using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TypicalTechTools.DataAccess;
using TypicalTechTools.Models;

namespace TypicalTechTools.Models.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddComment(Comment comment)
        {
            _context.Comments.Add(comment);
            _context.SaveChanges();
        }

        public void DeleteComment(int commentId)
        {
            var comment = _context.Comments.Find(commentId);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Comment> GetAllComments(int productId)
        {
            return _context.Comments.Where(c => c.ProductId == productId).ToList();
        }

        public Comment GetCommentById(int commentId)
        {
            return _context.Comments.FirstOrDefault(c => c.CommentId == commentId);
        }

        public void UpdateComment(Comment comment)
        {
            _context.Comments.Update(comment);
            _context.SaveChanges();
        }
    }
}
