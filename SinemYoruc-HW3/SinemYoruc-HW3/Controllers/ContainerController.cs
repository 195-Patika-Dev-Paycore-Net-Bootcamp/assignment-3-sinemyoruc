using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SinemYoruc_HW3.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    public class ContainerController : ControllerBase
    {
        private readonly IMapperSession session;
        public ContainerController(IMapperSession session)
        {
            this.session = session;
        }


        [HttpGet]
        public List<Container> Get()
        {
            var response = session.Containers.ToList();
            return response;
        }

        [HttpPost]
        public void Post([FromBody] Container container)
        {
            try
            {
                session.BeginTransaction();
                session.Save(container);
                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Container Insert Error");
            }
            finally
            {
                session.CloseTransaction();
            }
        }

        [HttpPut]
        public ActionResult<Container> Put([FromBody] Container request)
        {
            Container container = session.Containers.Where(x => x.Id == request.Id).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                container.ContainerName = request.ContainerName != default ? request.ContainerName : container.ContainerName;
                container.Latitude = request.Latitude != default ? request.Latitude : container.Latitude;
                container.Longitude = request.Longitude != default ? request.Longitude : container.Longitude;

                session.Update(container);

                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Container Updated Error");
            }
            finally
            {
                session.CloseTransaction();
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult<Container> Delete(int id)
        {
            Container container = session.Containers.Where(x => x.Id == id).FirstOrDefault();
            if (container == null)
            {
                return NotFound("Container is not found");
            }

            try
            {
                session.BeginTransaction();
                session.Delete(container);
                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Container Deleted Error");
            }
            finally
            {
                session.CloseTransaction();
            }

            return Ok();
        }

    }
}
