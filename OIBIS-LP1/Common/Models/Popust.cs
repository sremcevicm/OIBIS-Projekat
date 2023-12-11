using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Models
{
    public class Popust
    {
        int id;
        int procenat;

        public Popust(int procenat)
        {
            this.Procenat = procenat;
        }

        public int Procenat { get => procenat; set => procenat = value; }
    }
}
