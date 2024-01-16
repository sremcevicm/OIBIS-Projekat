namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class izmenaKorisnika : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Korisniks", "ImeKorisnika", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Korisniks", "ImeKorisnika");
        }
    }
}
