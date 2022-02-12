using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AfeiModel
{
    public static class ToDB
    {
        /// <summary>
        /// 配置项方法
        /// </summary>
        public static System.Collections.Specialized.NameValueCollection appsetting = ConfigurationManager.AppSettings;
        /// <summary>
        /// 服务器地址
        /// </summary>
        public static string severnm = appsetting["severnm"] ?? "";
        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string dbnm = appsetting["dbnm"] ?? "";
        /// <summary>
        /// 用户名
        /// </summary>
        public static string usernm = appsetting["usernm"] ?? "";
        /// <summary>
        /// 用户密码
        /// </summary>
        public static string userpwd = appsetting["userpwd"] ?? "";
        /// <summary>
        /// 普通连接字符串
        /// </summary>
        public static string consql = string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", severnm, dbnm, usernm, userpwd);
        /// <summary>
        /// 输入对应的sql语句，再用T承接之后形成泛型集合返回
        /// </summary>
        /// <typeparam name="T">用来接收的实体</typeparam>
        /// <param name="sql">查询语句</param>
        /// <returns></returns>
        public static List<T> Getdblist<T>(string sql) where T : class, new()
        {
            var dt = Select(sql).Tables[0];
            var list = new List<T>();
            PropertyInfo[] plist = typeof(T).GetProperties();
            //声明一个临时变量
            string pnm = string.Empty;
            foreach (DataRow dr in dt.Rows)
            {
                T t = new T();
                //获取此模型的公共属性
                PropertyInfo[] pps = t.GetType().GetProperties();
                //遍历该对象的公共属性
                foreach (PropertyInfo pi in pps)
                {
                    //将属性名称复制给临时变量
                    pnm = pi.Name;
                    //检查Datatable中是否包含此列
                    if (dt.Columns.Contains(pnm))
                    {
                        //判断类型
                        var type = pi.PropertyType.Name;
                        //取值
                        var value = dr[pnm];
                        //非空赋值,并且根据不同的数据类型
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(t, SetVal(value, type), null);
                        }
                    }

                }
                list.Add(t);
            }
            return list;
        }
        /// <summary>
        /// 将对应的值输出成对应类型的对象
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        public static object SetVal(object val, string type)
        {
            switch (type.ToLower())
            {
                case "string":
                    return Convert.ToString(val);
                case "int64":
                    return Convert.ToInt64(val);
                case "int32":
                    return Convert.ToInt32(val);
                case "int16":
                    return Convert.ToInt16(val);
                case "datetime":
                    return Convert.ToDateTime(val);
                case "decimal":
                    return Convert.ToDecimal(val);
                case "boolean":
                case "bool":
                    return Convert.ToBoolean(val);
                default:
                    return val.ToString();
            }
        }
        public static DataSet Select(string sql)
        {
            string strcon = consql;
            //创建连接
            SqlConnection con = new SqlConnection(strcon);
            //创建适配器
            SqlDataAdapter ada = new SqlDataAdapter(sql, con);
            //创建内存表
            DataSet ds = new DataSet();
            //往内存表里填数据
            ada.Fill(ds);
            //返回内存表
            return ds;
        }
        /// <summary>
        /// 将sql的数据类型转换成系统的数据类型
        /// </summary>
        /// <param name="type">类型名称</param>
        /// <returns></returns>
        public static string GetType(string type)
        {
            string reval = string.Empty;
            switch (type.ToLower())
            {
                case "int":
                    reval = "Int32";
                    break;
                case "text":
                    reval = "String";
                    break;
                case "bigint":
                    reval = "Int64";
                    break;
                case "binary":
                    reval = "System.Byte[]";
                    break;
                case "bit":
                    reval = "Boolean";
                    break;
                case "char":
                    reval = "String";
                    break;
                case "datetime":
                case "date":
                    reval = "System.DateTime";
                    break;
                case "decimal":
                    reval = "System.Decimal";
                    break;
                case "float":
                    reval = "System.Double";
                    break;
                case "image":
                    reval = "System.Byte[]";
                    break;
                case "money":
                    reval = "System.Decimal";
                    break;
                case "nchar":
                    reval = "String";
                    break;
                case "ntext":
                    reval = "String";
                    break;
                case "numeric":
                    reval = "System.Decimal";
                    break;
                case "nvarchar":
                    reval = "String";
                    break;
                case "real":
                    reval = "System.Single";
                    break;
                case "smalldatetime":
                    reval = "System.DateTime";
                    break;
                case "smallint":
                    reval = "Int16";
                    break;
                case "smallmoney":
                    reval = "System.Decimal";
                    break;
                case "timestamp":
                    reval = "System.DateTime";
                    break;
                case "tinyint":
                    reval = "System.Byte";
                    break;
                case "uniqueidentifier":
                    reval = "System.Guid";
                    break;
                case "varbinary":
                    reval = "System.Byte[]";
                    break;
                case "varchar":
                    reval = "String";
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = "String";
                    break;
            }
            return reval;
        }
    }
}
