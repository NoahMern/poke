using MongoDB.Driver;
using poke.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace poke.Services
{
    public class PokemonService : IPokemonService
    {
        private readonly IMongoCollection<Pokemon> _pokemons;

        public PokemonService(IConfiguration settings)
        {
            var client = new MongoClient(settings["MongoDbSettings:ConnectionString"]);
            var database = client.GetDatabase(settings["MongoDbSettings:DatabaseName"]);
            _pokemons = database.GetCollection<Pokemon>(settings["MongoDbSettings:CollectionName"]);
        }

        // Get all Pokemons (async)
        public async Task<List<Pokemon>> GetPokemonsAsync()
        {
            return await _pokemons.Find(pokemon => true).ToListAsync();
        }

        // Get Pokemon by ID (async)
        public async Task<Pokemon> GetPokemonByIdAsync(string id)
        {
            return await _pokemons.Find(pokemon => pokemon.Id == id).FirstOrDefaultAsync();
        }

        // Get Pokemon by Name (async)
        public async Task<Pokemon> GetPokemonByNameAsync(string name)
        {
            return await _pokemons.Find(pokemon => pokemon.Name == name).FirstOrDefaultAsync();
        }

        // Add new Pokemon (async)
        public async Task<Pokemon> AddPokemonAsync(Pokemon newPokemon)
        {
            newPokemon.Id = Guid.NewGuid().ToString();
            await _pokemons.InsertOneAsync(newPokemon);
            return newPokemon;
        }

        // Update Pokemon (async)
        public async Task<Pokemon> UpdatePokemonAsync(string id, Pokemon updatedPokemon)
        {
            updatedPokemon.Id = id;
            await _pokemons.ReplaceOneAsync(pokemon => pokemon.Id == id, updatedPokemon);
            return updatedPokemon;
        }

        // Delete Pokemon (async)
        public async Task<bool> DeletePokemonAsync(string id)
        {
            var result = await _pokemons.DeleteOneAsync(pokemon => pokemon.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
