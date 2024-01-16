namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tabelaKorisnik : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Korisniks",
                c => new
                    {
                        KorisnikId = c.Int(nullable: false, identity: true),
                        StanjeNaRacunu = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.KorisnikId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Korisniks");
        }
    }
}
