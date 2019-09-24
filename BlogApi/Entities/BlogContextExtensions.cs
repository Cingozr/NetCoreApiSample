using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApi.Entities
{
    public static class BlogContextExtensions
    {

        public static void EnsureSeedDataForContext(this BlogContext context)
        {
            context.Authors.RemoveRange(context.Authors);
            context.Articles.RemoveRange(context.Articles);

            var authors = new List<Author>()
            {
                new Author
                {
                    Id = new Guid("389e0c19-5e15-4676-86c6-dcbad7787959"),
                    Name = "Recai",
                    Surname = "Cingoz",
                    Age = new DateTimeOffset(new DateTime(1990,12,12)),
                    Articles = new List<Article>()
                    {
                        new Article
                        {
                            Id = new Guid("45064fee-4a3a-4e9b-a612-d6d0da7c4fe1"),
                            Title = "How to Install Entity Framework",
                            Description = "The Entity Framework Tools for Visual Studio include the EF Designer and the EF Model Wizard and are required for the database first and model first workflows. EF Tools are included in all recent versions of Visual Studio. If you perform a custom install of Visual Studio you will need to ensure that the item ",
                            Category = "Entity Framework"
                        }
                    }
                },
                new Author
                {
                    Id = new Guid("fbdf355f-e56c-43fb-b15b-ed06a9ce1646"),
                    Name = "Gamze",
                    Surname = "Aslan",
                    Age = new DateTimeOffset(new DateTime(1993,4,24)),
                    Articles = new List<Article>()
                    {
                        new Article
                        {
                            Id = new Guid("2369439e-72cd-4f91-b5ab-0e45f28cfee5"),
                            Title = "SQL Server instance by using SQL Server Management Studio",
                            Description = "This tutorial teaches you how to use SQL Server Management Studio (SSMS) to connect to your SQL Server instance and run some basic Transact-SQL (T-SQL) commands. The article demonstrates how to follow the below steps",
                            Category = "SQL Server"
                        },
                        new Article
                        {
                            Id = new Guid("a1e25bef-a8e9-4856-ab48-593138deae45"),
                            Title = "What is Azure DevOps?",
                            Description = "Azure DevOps provides developer services to support teams to plan work, collaborate on code development, and build and deploy applications. Developers can work in the cloud using Azure DevOps Services or on-premises using Azure DevOps Server, formerly named Visual Studio Team Foundation Server (TFS)",
                            Category = "Azure DevOps"
                        }
                    }
                }
            };

            context.Authors.AddRange(authors);
            context.SaveChanges();

        }
    }
}


