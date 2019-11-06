using Newtonsoft.Json;
using Sage.Comtax.API.Domain.Core.Modelos;
using System;
using System.Collections.Generic;
using System.Text;

namespace TesteViaVarejo.Domain.Entidades
{
    public class Usuario 
    {
        [JsonProperty("Id")]
        public int Id { get; set; }
        [JsonProperty("Nome")]
        public string Nome { get; set; }
        [JsonProperty("EMail")]
        public string EMail { get; set; }
        [JsonProperty("SenhaHash")]
        public string SenhaHash { get; set; }

    }
}
