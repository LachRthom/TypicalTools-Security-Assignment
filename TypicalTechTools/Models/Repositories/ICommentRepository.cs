namespace TypicalTechTools.Models.Repositories
{
    public interface ICommentRepository
    {
        IEnumerable<Comment> GetAllComments(int productId);
        Comment GetCommentById(int commentId);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
        void DeleteComment(int commentId);
    }
}
