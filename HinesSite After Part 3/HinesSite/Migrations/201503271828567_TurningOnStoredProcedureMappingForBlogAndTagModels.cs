namespace HinesSite.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class TurningOnStoredProcedureMappingForBlogAndTagModels : DbMigration
    {
        public override void Up()
        {
            CreateStoredProcedure(
                "dbo.Blogpost_Insert",
                p => new
                    {
                        Title = p.String(maxLength: 75),
                        Subtitle = p.String(maxLength: 150),
                        Body = p.String(maxLength: 1000),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        CommentId = p.Int(),
                        TagId = p.Int(),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"INSERT [dbo].[Blogpost]([Title], [Subtitle], [Body], [CreatedOn], [ModifiedOn], [CommentId], [TagId], [UserId])
                      VALUES (@Title, @Subtitle, @Body, @CreatedOn, @ModifiedOn, @CommentId, @TagId, @UserId)

                      DECLARE @BlogpostId int
                      SELECT @BlogpostId = [BlogpostId]
                      FROM [dbo].[Blogpost]
                      WHERE @@ROWCOUNT > 0 AND [BlogpostId] = scope_identity()

                      SELECT t0.[BlogpostId], t0.[RowVersion]
                      FROM [dbo].[Blogpost] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[BlogpostId] = @BlogpostId"
            );

            CreateStoredProcedure(
                "dbo.Blogpost_Update",
                p => new
                    {
                        BlogpostId = p.Int(),
                        Title = p.String(maxLength: 75),
                        Subtitle = p.String(maxLength: 150),
                        Body = p.String(maxLength: 1000),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        CommentId = p.Int(),
                        TagId = p.Int(),
                        UserId = p.String(maxLength: 128),
                    },
                body:
                    @"UPDATE [dbo].[Blogpost]
                      SET [Title] = @Title, [Subtitle] = @Subtitle, [Body] = @Body, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [CommentId] = @CommentId, [TagId] = @TagId, [UserId] = @UserId
                      WHERE (([BlogpostId] = @BlogpostId) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))

                      SELECT t0.[RowVersion]
                      FROM [dbo].[Blogpost] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[BlogpostId] = @BlogpostId"
            );

            CreateStoredProcedure(
                "dbo.Blogpost_Delete",
                p => new
                    {
                        BlogpostId = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Blogpost]
                      WHERE (([BlogpostId] = @BlogpostId) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );

            CreateStoredProcedure(
                "dbo.Tag_Insert",
                p => new
                    {
                        Name = p.String(maxLength: 30),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        BlogpostId = p.Int(),
                    },
                body:
                    @"INSERT [dbo].[Tag]([Name], [CreatedOn], [ModifiedOn], [BlogpostId])
                      VALUES (@Name, @CreatedOn, @ModifiedOn, @BlogpostId)

                      DECLARE @TagId int
                      SELECT @TagId = [TagId]
                      FROM [dbo].[Tag]
                      WHERE @@ROWCOUNT > 0 AND [TagId] = scope_identity()

                      SELECT t0.[TagId], t0.[RowVersion]
                      FROM [dbo].[Tag] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[TagId] = @TagId"
            );

            CreateStoredProcedure(
                "dbo.Tag_Update",
                p => new
                    {
                        TagId = p.Int(),
                        Name = p.String(maxLength: 30),
                        CreatedOn = p.DateTime(),
                        ModifiedOn = p.DateTime(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                        BlogpostId = p.Int(),
                    },
                body:
                    @"UPDATE [dbo].[Tag]
                      SET [Name] = @Name, [CreatedOn] = @CreatedOn, [ModifiedOn] = @ModifiedOn, [BlogpostId] = @BlogpostId
                      WHERE (([TagId] = @TagId) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))

                      SELECT t0.[RowVersion]
                      FROM [dbo].[Tag] AS t0
                      WHERE @@ROWCOUNT > 0 AND t0.[TagId] = @TagId"
            );

            CreateStoredProcedure(
                "dbo.Tag_Delete",
                p => new
                    {
                        TagId = p.Int(),
                        RowVersion_Original = p.Binary(maxLength: 8, fixedLength: true, storeType: "rowversion"),
                    },
                body:
                    @"DELETE [dbo].[Tag]
                      WHERE (([TagId] = @TagId) AND (([RowVersion] = @RowVersion_Original) OR ([RowVersion] IS NULL AND @RowVersion_Original IS NULL)))"
            );

        }

        public override void Down()
        {
            DropStoredProcedure("dbo.Tag_Delete");
            DropStoredProcedure("dbo.Tag_Update");
            DropStoredProcedure("dbo.Tag_Insert");
            DropStoredProcedure("dbo.Blogpost_Delete");
            DropStoredProcedure("dbo.Blogpost_Update");
            DropStoredProcedure("dbo.Blogpost_Insert");
        }
    }
}
