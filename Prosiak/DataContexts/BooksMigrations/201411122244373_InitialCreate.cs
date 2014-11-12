namespace Prosiak.DataContexts.BooksMigrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 60),
                        Author = c.String(nullable: false, maxLength: 30),
                        Isbn = c.String(maxLength: 13),
                        Category = c.Int(nullable: false),
                        Owner = c.String(nullable: false, maxLength: 30),
                        Reader = c.String(maxLength: 30),
                        Status = c.Boolean(nullable: false),
                        Timestamp = c.DateTime(nullable: false),
                        UsrStamp = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.BookCards",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        BorrowDate = c.DateTime(nullable: false),
                        ReturnDate = c.DateTime(),
                        Reader = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.BookCards");
            DropTable("dbo.Books");
        }
    }
}
