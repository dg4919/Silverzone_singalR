namespace SilverzoneERP.Context.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class dfg1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CounterCustomer", "StockId", "dbo.Stock");
            DropIndex("dbo.CounterCustomer", new[] { "StockId" });
            AddColumn("dbo.CounterCustomer", "StockInfo_Id", c => c.Long());
            CreateIndex("dbo.CounterCustomer", "StockInfo_Id");
            AddForeignKey("dbo.CounterCustomer", "StockInfo_Id", "dbo.Stock_Master", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CounterCustomer", "StockInfo_Id", "dbo.Stock_Master");
            DropIndex("dbo.CounterCustomer", new[] { "StockInfo_Id" });
            DropColumn("dbo.CounterCustomer", "StockInfo_Id");
            CreateIndex("dbo.CounterCustomer", "StockId");
            AddForeignKey("dbo.CounterCustomer", "StockId", "dbo.Stock", "Id", cascadeDelete: true);
        }
    }
}
