using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System;

namespace ControleFinanceiro.Models 
{
    public class Usuario
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("nome")]
        [Required]
        public string Nome { get; set; }
        
        [BsonElement("sobrenome")]
        public string Sobrenome { get; set; }
        
        [BsonElement("login")]
        [BsonIgnoreIfNull]
        public string Login { get; set; }
        
        [BsonElement("email")]
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [BsonElement("cpf")]
        [Required]
        public string CPF { get; set; }

        [BsonElement("data_nascimento")]
        [Required]
        public DateTime DataNascimento { get; set; }        
    }

    public class UsuarioSec : Usuario
    {   
        [BsonElement("ativo")]
        public bool Ativo { get; set; } = true;

        [BsonElement("senha")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string Senha { get; set; }       

    }
}