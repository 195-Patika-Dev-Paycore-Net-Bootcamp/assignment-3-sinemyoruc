using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SinemYoruc_HW3.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ClusteringController : ControllerBase
    {
        private readonly IMapperSession session;
        public ClusteringController(IMapperSession session)
        {
            this.session = session;
        }

        [HttpGet]
        public ActionResult<List<Container>> GetByClustering(int id, int n)
        {
            var query = session.Containers.Where(x => x.VehicleId == id).ToList()
                .Select((x, i) => new { Index = i, value = x })
                .GroupBy(x => x.Index % n)
                .Select(x => x.Select(v => v.value).ToList())
                .ToList();

            return Ok(query);
        }
    }
}
