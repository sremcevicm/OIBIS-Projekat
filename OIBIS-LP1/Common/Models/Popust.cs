using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Popust
    {
        [Key]
        public int PopustId { get; set; }

        public int Procenat { get; set; }

        public Popust()
        {
            // Pravilo koje zahteva postojanje parametraless konstruktora za Entity Framework
        }

        public Popust(int procenat)
        {
            this.Procenat = procenat;
        }
    }
}
