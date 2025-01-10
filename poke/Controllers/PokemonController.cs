using Microsoft.AspNetCore.Mvc;
using poke.Models;
using poke.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace poke.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonService _pokemonService;

        public PokemonController(IPokemonService pokemonService)
        {
            _pokemonService = pokemonService;
        }

        // Get all Pokemons (async)
        [HttpGet]
        public async Task<ActionResult<List<Pokemon>>> Get()
        {
            var pokemons = await _pokemonService.GetPokemonsAsync();
            return Ok(pokemons);
        }

        // Get Pokemon by ID (async)
        [HttpGet("{id}")]
        public async Task<ActionResult> GetPokemon(string id)
        {
            try
            {
                var pokemon = await _pokemonService.GetPokemonByIdAsync(id);
                if (pokemon == null)
                    return NotFound("Pokemon not found");

                return Ok(pokemon);
            }
            catch
            {
                return NotFound("Pokemon not found");
            }
        }

        // Get Pokemon by Name (async)
        [HttpGet("search/{name}")]
        public async Task<ActionResult<Pokemon>> GetPokemonByName(string name)
        {
            var pokemon = await _pokemonService.GetPokemonByNameAsync(name);
            if (pokemon == null)
                return NotFound("Pokemon not found");

            return Ok(pokemon);
        }

        // Add new Pokemon (async)
        [HttpPost]
        public async Task<ActionResult<Pokemon>> AddPokemon(Pokemon newPokemon)
        {
            var pokemon = await _pokemonService.AddPokemonAsync(newPokemon);
            return CreatedAtAction(nameof(GetPokemon), new { id = pokemon.Id }, pokemon);
        }

        // Update Pokemon (async)
        [HttpPut("{id}")]
        public async Task<ActionResult<Pokemon>> UpdatePokemon(string id, Pokemon updatedPokemon)
        {
            var pokemon = await _pokemonService.UpdatePokemonAsync(id, updatedPokemon);
            if (pokemon == null)
                return NotFound("Pokemon not found");

            return Ok(pokemon);
        }

        // Delete Pokemon (async)
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePokemon(string id)
        {
            var result = await _pokemonService.DeletePokemonAsync(id);
            if (!result)
                return NotFound("Pokemon not found");

            return NoContent();
        }
    }
}
