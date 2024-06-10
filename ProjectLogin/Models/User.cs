using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

public class NewsArticle
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; }

    [Required]
    public string Content { get; set; }

    public DateTime PublicationDate { get; set; } = DateTime.Now;

    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public string Preview
    {
        get
        {
            var endOfFirstSentence = Content.IndexOf('.') + 1;
            return endOfFirstSentence > 0 ? Content.Substring(0, endOfFirstSentence) : Content;
        }
    }
}


public class Comment
{
    public int Id { get; set; }

    [Required]
    [StringLength(500)]
    public string Content { get; set; }

    public DateTime CommentDate { get; set; } = DateTime.Now;

    [Required]
    public string Author { get; set; }

    public int NewsArticleId { get; set; }
    public NewsArticle NewsArticle { get; set; }
}


public class User
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    public bool IsAdmin { get; set; }
}


public class AdminPanelViewModel
{
    public List<NewsArticle> NewsArticles { get; set; }
    public List<User> Users { get; set; }
    public List<Comment> Comments { get; set; }
}
