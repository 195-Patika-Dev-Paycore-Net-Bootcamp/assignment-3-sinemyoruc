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
        public List<Container> Get() //Tum containerlari getiren method
        {
            var response = session.Containers.ToList();
            return response;
        }


        [HttpGet("{id}")]
        public ActionResult<List<Container>> GetById(int id) // girilen vehicleId ye göre containerlari listele
        {
            try
            {
                Vehicle vehicle = session.Vehicles.Where(x => x.Id == id).FirstOrDefault();
                var container = session.Containers.Where(x => x.VehicleId == vehicle.Id).ToList();
                return container;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Vehicle is not found");
                return NotFound("Vehicle is not found");
            }

        }


        [HttpPost]
        public void Post([FromBody] Container container) //Container ekleme
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
        public ActionResult<Container> Put([FromBody] Container request) //Container guncelleme
        {
            Container container = session.Containers.Where(x => x.Id == request.Id).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                //Id ve vehicleId alanlari guncellenmedi

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
        public ActionResult<Container> Delete(int id) //Container silme
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
