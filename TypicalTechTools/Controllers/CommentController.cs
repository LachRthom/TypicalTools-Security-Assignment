using Microsoft.AspNetCore.Mvc;
using TypicalTechTools.Models;
using TypicalTechTools.Models.Repositories;
using System;
using Microsoft.AspNetCore.Authorization;
using Ganss.Xss;

namespace TypicalTools.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly HtmlSanitizer _sanitizer;

        public CommentController(ICommentRepository commentRepository, HtmlSanitizer sanitizer)
        {
            _commentRepository = commentRepository;
            _sanitizer = sanitizer;
        }

        [HttpGet]
        public IActionResult CommentList(int id)
        {
            var comments = _commentRepository.GetAllComments(id);
            return View(comments);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult AddComment(int productId)
        {
            var comment = new Comment { ProductId = productId };
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult AddComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                return View(comment);
            }

            // Retrieve the session ID from the current session
            string sessionId = HttpContext.Session.GetString("SessionId");

            // If the session ID doesn't exist, generate a new one
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                // Store the session ID in the session to maintain consistency throughout the user's session
                HttpContext.Session.SetString("SessionId", sessionId);
            }

            // Set the session ID in the comment
            comment.SessionId = sessionId;

            // Set the CreatedDate for the comment
            comment.CreatedDate = DateTime.Now;

            // Sanitize comment text
            comment.CommentText = _sanitizer.Sanitize(comment.CommentText);

            _commentRepository.AddComment(comment);

            return RedirectToAction("Index", "Product");
        }

        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult RemoveComment(int commentId)
        {
            _commentRepository.DeleteComment(commentId);
            return RedirectToAction("Index", "Product");
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult EditComment(int commentId)
        {
            var comment = _commentRepository.GetCommentById(commentId);
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN, GUEST")]
        public IActionResult EditComment(Comment comment)
        {
            if (!ModelState.IsValid)
            {
                // Model state is not valid, return the view with validation errors
                return View(comment);
            }

            // Retrieve the session ID from the current session 
            string sessionId = HttpContext.Session.GetString("SessionId");

            // Set the session ID in the comment
            comment.SessionId = sessionId;

            // Sanitize comment text before updating
            comment.CommentText = _sanitizer.Sanitize(comment.CommentText);

            _commentRepository.UpdateComment(comment);

            return RedirectToAction("Index", "Product");
        }
    }
}
