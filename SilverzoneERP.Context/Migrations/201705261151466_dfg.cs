namespace SilverzoneERP.Context.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class dfg : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CounterCustomer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        name = c.String(),
                        address = c.String(),
                        mobile = c.String(),
                        emailId = c.String(),
                        PaymentMode = c.Int(nullable: false),
                        StockId = c.Long(nullable: false),
                        CreationDate = c.DateTime(nullable: false),
                        CreatedBy = c.Long(nullable: false),
                        UpdationDate = c.DateTime(nullable: false),
                        UpdatedBy = c.Long(nullable: false),
                        RowVersion = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Stock", t => t.StockId, cascadeDelete: true)
                .Index(t => t.StockId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CounterCustomer", "StockId", "dbo.Stock");
            DropIndex("dbo.CounterCustomer", new[] { "StockId" });
            DropTable("dbo.CounterCustomer");
        }
    }
}
