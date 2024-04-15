using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

using Aplicativo_Alquiler_Vehiculos.Models;
using System.Data.SqlClient;
using System.Data;


namespace Aplicativo_Alquiler_Vehiculos.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Server=localhost;Database=mydb;User Id = root; Password=andresramos1221";

        // GET: login
        public ActionResult login()
        {
            return View();
        }

        // GET: registro
        public ActionResult registro()
        {
            return View();
        }


        [HttpPost]
        public ActionResult registro(DataUsers usuario) {
            bool registrado;
            string mensaje;
            usuario.pass_usser = EncryptToSha256(usuario.pass_usser);

            using (SqlConnection cn = new SqlConnection(cadena)) {
                SqlCommand cmd = new SqlCommand("RegistrarUsuario", cn);

                cmd.Parameters.AddWithValue("p_num_documento",usuario.num_identidad);
                cmd.Parameters.AddWithValue("p_nombres",usuario.nombres);
                cmd.Parameters.AddWithValue("p_apellidos",usuario.apellidos);
                cmd.Parameters.AddWithValue("p_sexo",usuario.sexo);
                cmd.Parameters.AddWithValue("p_telefono",usuario.sexo);
                cmd.Parameters.AddWithValue("p_correo",usuario.correo);
                cmd.Parameters.AddWithValue("p_pass_user",usuario.pass_usser);
                cmd.Parameters.AddWithValue("p_tipo_usuario",usuario.tipo_usuario);
                cmd.Parameters.AddWithValue("p_fk_tp_documento",usuario.tipo_documento);

                cmd.Parameters.Add("registrado",SqlDbType.Int).Direction=ParameterDirection.Output;
                cmd.Parameters.Add("mensaje", SqlDbType.VarChar,255).Direction = ParameterDirection.Output;
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                cmd.ExecuteNonQuery();

                registrado = Convert.ToBoolean(cmd.Parameters["registrado"].Value);
                mensaje = cmd.Parameters["registrado"].Value.ToString();

            }

            ViewData["Mensaje"] = mensaje;

            if (registrado == true)
            {
                return RedirectToAction("login", "AccesoController");
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult login(DataUsers usuario)
        {
            usuario.pass_usser = EncryptToSha256(usuario.pass_usser);
            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("TraerDatosUsuario", cn);

                cmd.Parameters.AddWithValue("p_num_documento", usuario.num_identidad);
                cmd.Parameters.AddWithValue("p_pass_user", usuario.pass_usser);

                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();
                usuario.num_identidad = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }

            if (usuario.num_identidad!=0)
            {
                Session["usuario"] = usuario;
                return RedirectToAction("Index","Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no registrado";
                return View();

            }

        }



        public static string EncryptToSha256(string text) {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(text));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }


    }
}