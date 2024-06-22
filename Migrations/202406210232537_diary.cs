namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class diary : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diaries",
                c => new
                    {
                        content_Id = c.Int(nullable: false, identity: true),
                        title = c.String(),
                        diary_body = c.String(),
                        Post_date = c.DateTime(nullable: false),
                        comment = c.String(),
                        studentId = c.Int(nullable: false),
                        teacherId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.content_Id)
                .ForeignKey("dbo.Students", t => t.studentId, cascadeDelete: true)
                .ForeignKey("dbo.Teachers", t => t.teacherId, cascadeDelete: true)
                .Index(t => t.studentId)
                .Index(t => t.teacherId);
            
            AddColumn("dbo.Students", "Diary_content_Id", c => c.Int());
            AddColumn("dbo.Teachers", "Diary_content_Id", c => c.Int());
            CreateIndex("dbo.Students", "Diary_content_Id");
            CreateIndex("dbo.Teachers", "Diary_content_Id");
            AddForeignKey("dbo.Students", "Diary_content_Id", "dbo.Diaries", "content_Id");
            AddForeignKey("dbo.Teachers", "Diary_content_Id", "dbo.Diaries", "content_Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Teachers", "Diary_content_Id", "dbo.Diaries");
            DropForeignKey("dbo.Diaries", "teacherId", "dbo.Teachers");
            DropForeignKey("dbo.Students", "Diary_content_Id", "dbo.Diaries");
            DropForeignKey("dbo.Diaries", "studentId", "dbo.Students");
            DropIndex("dbo.Teachers", new[] { "Diary_content_Id" });
            DropIndex("dbo.Students", new[] { "Diary_content_Id" });
            DropIndex("dbo.Diaries", new[] { "teacherId" });
            DropIndex("dbo.Diaries", new[] { "studentId" });
            DropColumn("dbo.Teachers", "Diary_content_Id");
            DropColumn("dbo.Students", "Diary_content_Id");
            DropTable("dbo.Diaries");
        }
    }
}
