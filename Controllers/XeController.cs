using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ETC_Management_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class XeController : ControllerBase
    {
        private string connStr = "Server=.\\SQLEXPRESS;Database=ETC_Management;Integrated Security=True;TrustServerCertificate=True;";

        [HttpGet("{bienSo}")]
        public IActionResult CheckXe(string bienSo)
        {
            try
            {
                using var conn = new SqlConnection(connStr);
                conn.Open();

                using var cmd = new SqlCommand("XuLyQuaTram", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MaXeInput", bienSo);

                using var r = cmd.ExecuteReader();
                if (r.Read())
                {
                    return Ok(new
                    {
                        ketQua = r["KetQua"].ToString(),
                        soDu = Convert.ToDouble(r["SoDu"]),
                        mess = r["ThongBao"].ToString()
                    });
                }
                return NotFound(new { mess = "Không tìm thấy dữ liệu phản hồi" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}