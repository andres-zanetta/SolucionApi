using Microsoft.Extensions.Options;
using System.Data.SqlClient;
using System.Data;
using Proyecto_Modelo;


namespace ProyectoData
{
    public class Empleado_Data
    {

        private readonly ConnectionStrings conexiones;

        public Empleado_Data(IOptions<ConnectionStrings> options)
        {
            conexiones = options.Value; //recicbimos objetos del conectionstring
        }

        public async Task<List<Empleado>> Lista()
        {
            List<Empleado> lista = new List<Empleado>();

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))//creo una variable que me devuelve la conexxion de sql
            {
                await conexion.OpenAsync();
                SqlCommand cmd = new SqlCommand("sp_listaEmpleado", conexion);//pongo de esta forma para ejecutar los procedimientos almacenados
                cmd.CommandType = CommandType.StoredProcedure;

                using (var reader = await cmd.ExecuteReaderAsync())//creamos un lector que elecuta el proc de manera async//
                {
                    while (reader.Read()) //mientras nuestro lector lee los reusltados agregamos items a la lsita
                    {
                        lista.Add(new Empleado
                        {
                            IdEmpleado = Convert.ToInt32(reader["IdEmpleado"]),
                            NombreCompleto = reader["NombreCompleto"].ToString(),
                            Sueldo = Convert.ToDecimal(reader["Sueldo"]),
                            FechaContrato = reader["FechaContrato"].ToString(),
                            Departamento = new Departamento
                            {
                                IdDepartamento = Convert.ToInt32(reader["IdDepartamento"]),
                                Nombre = reader["NombreCompleto"].ToString()
                            }

                        });
                    }
                }
            }
            return lista;
        }

        public async Task<bool> Crear(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_crearEmpleado", conexion);//pagamos el nombre del procedimiento que tenemos en la base de datos
                cmd.Parameters.AddWithValue("@nombreCompleto",objeto.NombreCompleto);//le reenviamos todos los paramaetros que tenemos en la base de datos
                cmd.Parameters.AddWithValue("@IdDepartamento",objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@IdDepartamento",objeto.Sueldo);
                cmd.Parameters.AddWithValue("@IdDepartamento",objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

               try
               {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;          //devuelve la cant de filas afectadas
               }
               
               catch
               {
                    respuesta = false;
               }
            }
            return respuesta;
        }

        public async Task<bool> Editar(Empleado objeto)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_editarEmpleado", conexion);//pagamos el nombre del procedimiento que tenemos en la base de datos
                cmd.Parameters.AddWithValue("IdEmpleado", objeto.IdEmpleado);
                cmd.Parameters.AddWithValue("@nombreCompleto", objeto.NombreCompleto);//le reenviamos todos los paramaetros que tenemos en la base de datos
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Departamento!.IdDepartamento);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.Sueldo);
                cmd.Parameters.AddWithValue("@IdDepartamento", objeto.FechaContrato);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;          //devuelve la cant de filas afectadas
                }

                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public async Task<bool> Eliminar(int id)
        {
            bool respuesta = true;

            using (var conexion = new SqlConnection(conexiones.CadenaSQL))
            {
                SqlCommand cmd = new SqlCommand("sp_eliminarEmpleado", conexion);//pagamos el nombre del procedimiento que tenemos en la base de datos
                cmd.Parameters.AddWithValue("IdEmpleado",id);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    await conexion.OpenAsync();
                    respuesta = await cmd.ExecuteNonQueryAsync() > 0 ? true : false;          //devuelve la cant de filas afectadas
                }

                catch
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

    }
}
