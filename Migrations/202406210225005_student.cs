namespace PassionProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class student : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Students",
                c => new
                    {
                        studentId = c.Int(nullable: false, identity: true),
                        student_fname = c.String(),
                        student_lname = c.String(),
                    })
                .PrimaryKey(t => t.studentId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Students");
        }
    }
}
