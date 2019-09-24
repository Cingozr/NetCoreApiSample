using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Services
{
    public interface IArticleRepository
    {
        IEnumerable<Article> GetArticles();
        IEnumerable<Article> GetArticlesForAuthor(Guid authorId);
        Article GetArticle(Guid articleId);
        Article GetArticleForAuthor(Guid authorId, Guid articleId);
        void AddArticleForAuthor(Guid authorId, Article article);
        void UpdateArticleForAuthor(Article article);
        void DeleteArticle(Article article);
        bool Save();
    }
}
