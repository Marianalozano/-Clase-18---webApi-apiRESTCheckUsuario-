using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using apiRESTCheckUsuario.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Data;

namespace apiRESTCheckUsuario.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("check/usuario/spinusuario")]
        public clsApiStatus spInsUsuario([FromBody] clsUsuario modelo)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario(modelo.nombre, modelo.apellidoPaterno, modelo.apellidoMaterno, modelo.usuario, modelo.contrasena, modelo.ruta, modelo.tipo);
                ds = objUsuario.spInsUsuario();

                objRespuesta.statusExec = true;
                objRespuesta.ban = int.Parse(ds.Tables[0].Rows[0][0].ToString());

                objRespuesta.msg = objRespuesta.ban == 0 ? "Usuario registrado exitosamente" : "Usuario no registrado, verificar ..";
                jsonResp.Add("msgData", objRespuesta.msg);
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Falló la inserción de usuario";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }
        // Filtro para identificar
        [HttpGet]
        [Route("stack/usuario/vwrptusuario/{filtro}")]
        public clsApiStatus vwRptUsuario(string filtro)
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vwRptUsuario(filtro);

                objRespuesta.statusExec = true;
                objRespuesta.ban = ds.Tables[0].Rows.Count;
                objRespuesta.msg = "Usuarios consultados exitosamente";

                string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                jsonResp = JObject.Parse($"{{\"usuarios\": {jsonString}}}");
                objRespuesta.datos = jsonResp;
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al consultar usuarios";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }
        // Tipo de usuario
        [HttpGet]
        [Route("stack/usuario/vmwTipoUsuario")]
        public clsApiStatus vmwTipoUsuario()
        {
            clsApiStatus objRespuesta = new clsApiStatus();
            JObject jsonResp = new JObject();
            DataSet ds = new DataSet();

            try
            {
                clsUsuario objUsuario = new clsUsuario();
                ds = objUsuario.vmwTipoUsuario();

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    objRespuesta.statusExec = true;
                    objRespuesta.ban = ds.Tables[0].Rows.Count;
                    objRespuesta.msg = "Tipos de usuario consultados exitosamente";

                    string jsonString = JsonConvert.SerializeObject(ds.Tables[0], Formatting.Indented);
                    jsonResp = JObject.Parse($"{{\"tiposUsuario\": {jsonString}}}");
                    objRespuesta.datos = jsonResp;
                }
                else
                {
                    objRespuesta.statusExec = false;
                    objRespuesta.ban = 0;
                    objRespuesta.msg = "No se encontraron tipos de usuario";
                    jsonResp.Add("msgData", "La consulta no devolvió resultados");
                    objRespuesta.datos = jsonResp;
                }
            }
            catch (Exception ex)
            {
                objRespuesta.statusExec = false;
                objRespuesta.ban = -1;
                objRespuesta.msg = "Error al consultar tipos de usuario";
                jsonResp.Add("msgData", ex.Message.ToString());
                objRespuesta.datos = jsonResp;
            }

            return objRespuesta;
        }
    }
}
