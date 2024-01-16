namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Popusts",
                c => new
                    {
                        PopustId = c.Int(nullable: false, identity: true),
                        Procenat = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PopustId);
            
            CreateTable(
                "dbo.Projekcijas",
                c => new
                    {
                        ProjekcijaId = c.Int(nullable: false, identity: true),
                        Naziv = c.String(),
                        VremeProjekcije = c.DateTime(nullable: false),
                        Sala = c.Int(nullable: false),
                        CenaKarte = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.ProjekcijaId);
            
            CreateTable(
                "dbo.Rezervacijas",
                c => new
                    {
                        RezervacijaId = c.Int(nullable: false, identity: true),
                        IdProjekcije = c.Int(nullable: false),
                        VremeRezervacije = c.DateTime(nullable: false),
                        KolicinaKarata = c.Int(nullable: false),
                        StanjeRezervacije = c.Int(nullable: false),
                        Kreirao = c.String(),
                    })
                .PrimaryKey(t => t.RezervacijaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Rezervacijas");
            DropTable("dbo.Projekcijas");
            DropTable("dbo.Popusts");
        }
    }
}
