using RestaurantRater.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Http;

namespace RestaurantRater.Controllers
{
    public class RestaurantController : ApiController
    {
        private readonly RestaurantDbContext _context = new RestaurantDbContext();

        //POST
        [HttpPost]
        public async Task<IHttpActionResult> PostRestaurant(Restaurant restaurant)
        {
            if (ModelState.IsValid && restaurant != null)
            {
                _context.Restaurants.Add(restaurant);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest(ModelState);
        }

        //GET ALL
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            List<Restaurant> allRestaurants = await _context.Restaurants.ToListAsync();
            return Ok(allRestaurants);
        }

        //GET BY ID
        [HttpGet]
        public async Task<IHttpActionResult> GetById(int id)
        {
            Restaurant restaurantId = await _context.Restaurants.FindAsync(id);

            if (restaurantId == null)
            {
                return NotFound();
            }

            return Ok(restaurantId);
        }

        //PUT (Update)
        [HttpPut]
        public async Task<IHttpActionResult> UpdateRestaurant([FromUri]int id, [FromBody]Restaurant model)
        {
            if(ModelState.IsValid && model != null)
            {
                Restaurant restaurantEntity = await _context.Restaurants.FindAsync(id);

                if (restaurantEntity != null)
                {
                    restaurantEntity.Name = model.Name;
                    restaurantEntity.Rating = model.Rating;
                    restaurantEntity.Style = model.Style;
                    restaurantEntity.DollarSigns = model.DollarSigns;

                    await _context.SaveChangesAsync();

                    return Ok();
                }
                return BadRequest(ModelState);
            }

            return BadRequest(ModelState);
        }

        //DELETE BY ID
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteRestaurantById(int id)
        {
            Restaurant restaurantId = await _context.Restaurants.FindAsync(id);

            if (restaurantId == null)
            {
                return NotFound();
                //return BadRequest();
            }

            _context.Restaurants.Remove(restaurantId);

            if (await _context.SaveChangesAsync() == 1) 
            {
                return Ok();
            }

            return InternalServerError();
        }
    }
}
