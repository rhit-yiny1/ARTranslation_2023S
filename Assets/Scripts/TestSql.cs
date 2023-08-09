using System.Data;
using UnityEngine;

public class TestSql : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SqlAccess sql = new SqlAccess();
        string[] items = { "id", "bless", "state", "time", "name", "banji" };
        string[] col = { };
        string[] op = { };
        string[] val = { };
        DataSet ds = sql.SelectWhere("blessings", items, col, op, val);

        if(ds != null)
        {
            DataTable table = ds.Tables[0];
            foreach (DataRow row in table.Rows)
            {
                string str = "";
                foreach (DataColumn column in table.Columns)
                    str += row[column] + " ";
                Debug.Log(str);
            }
        }
    }
}
