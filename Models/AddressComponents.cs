using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManagementApp.Models
{
    public class AddressComponents
    {
        public string Street { get; set; }
        public string Number { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public override string ToString()
        {
            return $"{Street} {Number}, {City}, {State}, {Country}";
        }
    }
}
