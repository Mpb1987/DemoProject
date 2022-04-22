using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DemoProject.ApplicationCore.DTO
{
    public class CustomerDto
    {
        [JsonProperty("firstname")]
        public string FirstName { get; set; }
        [JsonProperty("surname")]
        public string Surname { get; set; }
    }
}
