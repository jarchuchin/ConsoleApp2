using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Web.Script.Serialization;


namespace Componentes
{
    public class Utils
    {


  

         public static  string Convert_DataRowToJson(DataRow datarow)
        {
            var dict = new Dictionary<string, object>();
            foreach (DataColumn col in datarow.Table.Columns)
            {
                dict.Add(col.ColumnName, datarow[col]);
            }

            var jsSerializer = new JavaScriptSerializer();

            return jsSerializer.Serialize(dict);
        }
    }
}
