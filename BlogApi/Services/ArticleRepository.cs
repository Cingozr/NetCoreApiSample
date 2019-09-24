using BlogApi.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Services
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly BlogContext _blogContext;
        public ArticleRepository(BlogContext blogContext)
        {
            _blogContext = blogContext;
        }
        public void AddArticleForAuthor(Guid authorId, Article article)
        {
            var author = _blogContext.Authors.Where(x => x.Id == authorId).FirstOrDefault();
            if (author != null)
            {
                if (article.Id == Guid.Empty)
                {
                    article.Id = Guid.NewGuid();
                }

                author.Articles.Add(article);
            }
        }

        public void DeleteArticle(Article article)
        {
            _blogContext.Articles.Remove(article);
        }

        public Article GetArticle(Guid articleId)
        {
            return _blogContext.Articles.Where(x => x.Id == articleId).FirstOrDefault();
        }

        public Article GetArticleForAuthor(Guid authorId, Guid articleId)
        {
            return _blogContext.Articles
                .Where(x => x.Id == articleId && x.Author.Id == authorId)
                .FirstOrDefault();
        }

        public IEnumerable<Article> GetArticles()
        {
            return _blogContext.Articles.ToList();
        }

        public IEnumerable<Article> GetArticlesForAuthor(Guid authorId)
        {
            var model = _blogContext.Articles
                .Where(x => x.AuthorId == authorId)
                .Include(x => x.Author)
                .ToList();

            return model;
        }

        public bool Save()
        {
            return _blogContext.SaveChanges() > 0;
        }

        public void UpdateArticleForAuthor(Article article)
        {
            throw new NotImplementedException();
        }
    }
}
