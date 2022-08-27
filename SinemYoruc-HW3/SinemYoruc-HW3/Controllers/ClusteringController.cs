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
            var listCount = session.Containers.Where(x => x.VehicleId == id).Count();
            int result = listCount / n;
            var query = session.Containers.Where(x => x.VehicleId == id).GroupBy(x => new { result, x.VehicleId });
            /*
             ArrayList list = new ArrayList();
            for (int i = 0; i < result; i++)
            {
                foreach (var item in query)
                {

                    list.Add(item);
                }
            }
            return Ok(list);*/
            return Ok(query);
        }
    }
}
