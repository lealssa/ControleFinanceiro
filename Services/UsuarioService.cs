using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using ControleFinanceiro.Models;
using System;

namespace ControleFinanceiro.Services
{
    public class UsuarioService
    {
        private readonly IMongoCollection<Usuario> _usuarios;

        public UsuarioService(IControleFinanceiroDatabaseSettings settings)
        {
            MongoClient client = new MongoClient(settings.ConnectionString);
            IMongoDatabase database = client.GetDatabase(settings.DatabaseName);

            _usuarios = database.GetCollection<Usuario>("usuarios");
        }

        public Usuario Get(string id)
        {
            ProjectionDefinition<Usuario> projection = Builders<Usuario>.Projection
            .Exclude("ativo")
            .Exclude("senha");

            Usuario usuario = _usuarios.Find<Usuario>(usuario => usuario.Id == id).Project(projection);

            return usuario;
        }
        
        public UsuarioSec Create(UsuarioSec usuario)
        {
            // Preenche o login com o email se vier vazio
            if (usuario.Login == null)
                usuario.Login = usuario.Email;

            // Busca usuário pelo email ou login
            Usuario usuarioExists = _usuarios.Find<Usuario>(
                usuarioToFind => 
                usuarioToFind.Email == usuario.Email || usuarioToFind.Login == usuario.Login
                ).FirstOrDefault();

            // Se ja existir, dispara exception
            // TODO: criar uma custom exception pra gerar o codigo HTTP 409
            // "conflict" no controller.
            if (usuarioExists != null)
                throw new ArgumentException($"User '{usuario.Email}' already exists");
                
            _usuarios.InsertOne(usuario);
            usuario.Senha = null;
            return usuario;
        }
        
        public void Update(string id, Usuario usuarioIn)
        {            
            _usuarios.ReplaceOne(usuario => usuario.Id == id, usuarioIn);
        }        
        public void Remove(Usuario usuarioIn) =>
            _usuarios.DeleteOne(usuario => usuario.Id == usuarioIn.Id);
        
        public void Remove(string id) =>
            _usuarios.DeleteOne(usuario => usuario.Id == id);
    }
}
