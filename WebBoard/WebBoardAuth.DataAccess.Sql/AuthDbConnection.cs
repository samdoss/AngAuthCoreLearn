using System.Data.SqlClient;

namespace WebBoardAuth.DataAccess.Sql
{
    public class AuthDbConnection
    {
        public SqlConnection bakserverWebBoardAuthConn { get; set; }
        public AuthDbConnection()
        {
            bakserverWebBoardAuthConn = new SqlConnection("Data Source=DESKTOP-KR0EJIO;Initial Catalog=WebBoardAuth;Persist Security Info=True;User ID=sa;Password=123456789");
        }
    }
}
