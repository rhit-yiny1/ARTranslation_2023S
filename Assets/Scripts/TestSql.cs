using System.Data;
using UnityEngine;

public class TestSql : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DataSet ds = null;
        SqlAccess sql = new SqlAccess();
        string[] items = { "id", "bless", "state", "time", "name", "banji" };
        string[] col = { };
        string[] op = { };
        string[] val = { "testInsert", "testInsertName" };
        string[] columns = { "bless", "name" };
        //DataSet ds = sql.SelectWhere("blessings", items, col, op, val);
        sql.InsertWhere("blessings", columns, val);

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
