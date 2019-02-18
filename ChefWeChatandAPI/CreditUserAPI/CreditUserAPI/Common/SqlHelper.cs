using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace CreditUserAPI.Common
{
    /// <summary>
    /// SqlHelper数据层
    /// </summary>
    public class SqlHelper : IDisposable
    {
        /// <summary>
        /// 数据库连接对象
        /// </summary>
        private SqlConnection connection;

        /// <summary>
        /// 初始化连接对象
        /// </summary>
        public SqlHelper()
        {
            connection = new SqlConnection(ConfigurationManager.ConnectionStrings["CreditSys"].ConnectionString);
        }
        public SqlHelper(IDbConnection con)
        {
            connection = con as SqlConnection;
        }
        /// <summary>
        /// 得到数据库连接
        /// </summary>
        public SqlConnection GetConnection
        {
            get { return connection; }
        }

        /// <summary>
        /// 将SqlParameters插入至SqlParameter.
        /// </summary>
        /// <param name="command">需要添加SqlParameters的Command</param>
        /// <param name="commandParameters">被添加到Command的SqlParameters数组</param>
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandParameters != null)
            {
                foreach (SqlParameter p in commandParameters)
                {
                    if (p != null)
                    {
                        if ((p.Direction == ParameterDirection.InputOutput ||
                             p.Direction == ParameterDirection.Input) &&
                            (p.Value == null))
                        {
                            p.Value = DBNull.Value;
                        }
                        command.Parameters.Add(p);
                    }
                }
            }
        }

        /// <summary>
        /// 将DataRow列的值插入到SqlParameters数组中
        /// </summary>
        /// <param name="commandParameters">SqlParameters数组</param>
        /// <param name="dataRow">DataTable数据行</param>
        protected static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            if ((commandParameters == null) || (dataRow == null))
            {
                return;
            }

            int i = 0;
            foreach (SqlParameter commandParameter in commandParameters)
            {
                if (commandParameter.ParameterName == null ||
                    commandParameter.ParameterName.Length <= 1)
                    throw new ArgumentNullException(
                        string.Format(
                            "Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: '{1}'.",
                            i, commandParameter.ParameterName));
                if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                i++;
            }
        }

        /// <summary>
        /// 将数组中的值复制到SqlParameters中
        /// </summary>
        /// <param name="commandParameters">SqlParameters数组</param>
        /// <param name="parameterValues">对象数组</param>
        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            if ((commandParameters == null) || (parameterValues == null))
            {
                return;
            }

            if (commandParameters.Length != parameterValues.Length)
            {
                throw new ArgumentException("Parameter count does not match Parameter Value count.");
            }

            for (int i = 0, j = commandParameters.Length; i < j; i++)
            {
                if (parameterValues[i] is IDbDataParameter)
                {
                    IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];
                    if (paramInstance.Value == null)
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = paramInstance.Value;
                    }
                }
                else if (parameterValues[i] == null)
                {
                    commandParameters[i].Value = DBNull.Value;
                }
                else
                {
                    commandParameters[i].Value = parameterValues[i];
                }
            }
        }

        /// <summary>
        /// 根据Connection, Transaction, CommandType and Parameters得到指定的Command
        /// </summary>
        /// <param name="command">指定的SqlParameter</param>
        /// <param name="transaction">有效的SqlTransaction, 或 'null'</param>
        /// <param name="commandType">CommandType类型(stored procedure, text, etc.)</param>
        /// <param name="commandText">The Stored procedure Name or T-SQL command</param>
        /// <param name="commandParameters">SqlParameters数组 或 'null'</param>
        /// <param name="mustCloseConnection"><c>true</c></param>
        private void PrepareCommand(SqlCommand command, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, out bool mustCloseConnection)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (commandText == null || commandText.Length == 0) throw new ArgumentNullException("commandText");

            if (connection.State != ConnectionState.Open)
            {
                mustCloseConnection = true;
                connection.Open();
            }
            else
            {
                mustCloseConnection = false;
            }

            command.Connection = connection;


            if (transaction != null)
            {
                if (transaction.Connection == null)
                    throw new ArgumentException(
                        "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
                command.Transaction = transaction;
            }

            command.CommandType = commandType;

            if (command.CommandType == CommandType.Text)
            {
                if (commandText.Substring(commandText.Length - 1) != ";")
                    commandText += ";";
            }

            command.CommandText = commandText;

            if (commandParameters != null)
            {
                AttachParameters(command, commandParameters);
            }

            return;
        }

        /// <summary>
        /// 通过提供SqlConnection执行SqlCommand. 
        /// </summary>
        /// <remarks>
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText)
        {
            return ExecuteNonQuery(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 使用提供的参数执行SqlCommand,带参数 
        /// </summary>
        /// <remarks>
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">SqlParamters数组</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                return -1;
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, (SqlTransaction)null, commandType, commandText, commandParameters, out mustCloseConnection);

                int retval = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                if (mustCloseConnection)
                {
                    connection.Close();
                }

                return retval;
            }
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,不提供SqlParameter . 
        /// </summary>
        /// <remarks>
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "PublishOrders",transaction);
        /// </remarks>        
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, SqlTransaction transaction)
        {
            return ExecuteNonQuery(commandType, commandText, (SqlParameter[])null, transaction);
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,提供SqlParameter
        /// </summary>
        /// <remarks>
        ///  int result = ExecuteNonQuery(CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24),trans);
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>返回影响的行数</returns>
        public int ExecuteNonQuery(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (transaction != null && transaction.Connection == null)
            {
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                int retval = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();
                return retval;
            }
        }

        /// <summary>
        /// 通过提供SqlConnection执行SqlCommand,不提供参数. 
        /// </summary>
        /// <remarks>
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText)
        {
            return ExecuteDataSet(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 通过提供SqlConnection执行SqlCommand,提供参数. 
        /// </summary>
        /// <remarks>
        ///  DataSet ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                return null;
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, (SqlTransaction)null, commandType, commandText, commandParameters,
                                out mustCloseConnection);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.Locale = CultureInfo.InvariantCulture;
                        da.Fill(ds);

                        cmd.Parameters.Clear();

                        if (mustCloseConnection)
                            connection.Close();
                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,不带参数. 
        /// </summary>
        /// <remarks>
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, SqlTransaction transaction)
        {
            return ExecuteDataSet(commandType, commandText, (SqlParameter[])null, transaction);
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,带参数.
        /// </summary>
        /// <remarks>
        ///  DataSet ds = ExecuteDataset(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExecuteDataSet(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.Locale = CultureInfo.InvariantCulture;
                        da.Fill(ds);

                        cmd.Parameters.Clear();

                        return ds;
                    }
                }
            }
        }

        /// <summary>
        /// 通过提供SqlConnection执行SqlCommand,不提供参数. 
        /// </summary>
        /// <remarks>
        ///  DataTable ds = ExecuteDataset(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText)
        {
            return ExecuteDataTable(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 通过提供SqlConnection执行SqlCommand,提供参数. 
        /// </summary>
        /// <remarks>
        ///  DataTable dt = ExecuteDataTable(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <returns>DataTable内存表</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                return null;
            }

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, (SqlTransaction)null, commandType, commandText, commandParameters,
                               out mustCloseConnection);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        dt.Locale = CultureInfo.InvariantCulture;
                        da.Fill(dt);

                        cmd.Parameters.Clear();

                        if (mustCloseConnection)
                            connection.Close();

                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,不带参数. 
        /// </summary>
        /// <remarks>
        ///  DataTable ds = ExecuteDataTable(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, SqlTransaction transaction)
        {
            return ExecuteDataTable(commandType, commandText, (SqlParameter[])null, transaction);
        }

        /// <summary>
        /// 通过提供SqlTransaction执行SqlCommand,带参数.
        /// </summary>
        /// <remarks>
        ///  DataTable ds = ExecuteDataTable(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        dt.Locale = CultureInfo.InvariantCulture;
                        da.Fill(dt);

                        cmd.Parameters.Clear();

                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// 调用ExecuteReader,通过提供CommandBehavior.
        /// </summary>
        /// <remarks>
        /// If we created and opened the connection, we want the connection to be closed when the DataReader is closed.
        /// 
        /// If the caller provided the connection, we want to leave it to them to manage.
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParameters or 'null'</param>
        /// <param name="transaction">一个有效的SqlTransaction, or 'null'</param>
        /// <param name="connectionOwnership">标识连接对象是由caller提供还是由SqlHelper提供</param>
        /// <returns>SqlDataReader结果集</returns>
        private SqlDataReader ExecuteReader(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction, SqlConnectionOwnership connectionOwnership)
        {
            if (connection == null)
            {
                return null;
            }
            bool mustCloseConnection = false;

            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    PrepareCommand(cmd, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                    SqlDataReader dataReader;

                    if (connectionOwnership == SqlConnectionOwnership.External)
                    {
                        dataReader = cmd.ExecuteReader();
                    }
                    else
                    {
                        dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    }

                    bool canClear = true;
                    foreach (SqlParameter commandParameter in cmd.Parameters)
                    {
                        if (commandParameter.Direction != ParameterDirection.Input)
                            canClear = false;
                    }

                    if (canClear)
                    {
                        cmd.Parameters.Clear();
                    }

                    return dataReader;
                }
            }
            catch
            {
                if (mustCloseConnection)
                    connection.Close();
                throw;
            }
        }

        /// <summary>
        /// 调用ExecuteReader,不带参数,此时属于外部调用者
        /// </summary>
        /// <remarks>
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>SqlDataReader结果集</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText)
        {
            return ExecuteReader(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 调用ExecuteReader,带参数,此时属于外部调用者
        /// </summary>
        /// <remarks>
        ///  SqlDataReader dr = ExecuteReader(conn, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <returns>SqlDataReader结果集</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            return ExecuteReader(commandType, commandText, commandParameters, (SqlTransaction)null, SqlConnectionOwnership.Internal);
        }

        /// <summary>
        /// 执行SqlCommand通过提供SqlTransactio,无参数,属于外部调用 
        /// </summary>
        /// <remarks>
        ///  SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>SqlDataReader结果集</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, SqlTransaction transaction)
        {
            return ExecuteReader(commandType, commandText, (SqlParameter[])null, transaction);
        }

        /// <summary>
        /// 执行SqlCommand通过提供SqlTransactio,带参数,属于外部调用
        /// </summary>
        /// <remarks>
        ///   SqlDataReader dr = ExecuteReader(trans, CommandType.StoredProcedure, "GetOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数SqlParamters</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>SqlDataReader结果集</returns>
        public SqlDataReader ExecuteReader(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(commandType, commandText, commandParameters, transaction, SqlConnectionOwnership.Internal);
        }

        /// <summary>
        /// 执行SqlCommand
        /// </summary>
        /// <remarks>
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <returns>返回1x1结果</returns>
        public object ExecuteScalar(CommandType commandType, string commandText)
        {
            return ExecuteScalar(commandType, commandText, (SqlParameter[])null);
        }

        /// <summary>
        /// 执行SqlCommand 
        /// </summary>
        /// <remarks>
        ///  int orderCount = (int)ExecuteScalar(conn, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">对象数组，将该值导入相应的参数中</param>
        /// <returns>返回1x1结果</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                return null;
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, (SqlTransaction)null, commandType, commandText, commandParameters,
                               out mustCloseConnection);

                object retval = cmd.ExecuteScalar();

                cmd.Parameters.Clear();

                if (mustCloseConnection)
                    connection.Close();

                return retval;
            }
        }

        /// <summary>
        /// 执行SqlCommand  
        /// </summary>
        /// <remarks>
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount");
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>返回1x1结果</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, SqlTransaction transaction)
        {
            return ExecuteScalar(commandType, commandText, (SqlParameter[])null, transaction);
        }

        /// <summary>
        /// 执行SqlCommand  
        /// </summary>
        /// <remarks>
        ///  int orderCount = (int)ExecuteScalar(trans, CommandType.StoredProcedure, "GetOrderCount", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="commandParameters">参数数组</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        /// <returns>返回1x1结果</returns>
        public object ExecuteScalar(CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException(
                    "The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                object retval = cmd.ExecuteScalar();

                cmd.Parameters.Clear();
                return retval;
            }
        }

        /// <summary>
        /// 利用TableMappings给DataSet绑定多个表，并给DataSet做相应的表名影射 
        /// </summary>
        /// <remarks>
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="dataSet">拥有多个结果集的Dataset</param>
        /// <param name="tableNames">需要影射的表名</param>    
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            FillDataSet(commandType, commandText, dataSet, tableNames, (SqlParameter[])null);
        }

        /// <summary>
        /// 利用TableMappings给DataSet绑定多个表，并给DataSet做相应的表名影射 
        /// </summary>
        /// <remarks>
        ///  FillDataset(conn, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="dataSet">拥有多个结果集的Dataset</param>
        /// <param name="tableNames">需要影射的表名</param>   
        /// <param name="commandParameters">参数数组</param>
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, SqlParameter[] commandParameters)
        {
            FillDataSet(commandType, commandText, dataSet, tableNames, commandParameters, null);
        }

        /// <summary>
        /// 利用TableMappings给DataSet绑定多个表，并给DataSet做相应的表名影射 
        /// </summary>
        /// <remarks>
        ///  FillDataset(trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"});
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="dataSet">拥有多个结果集的Dataset</param>
        /// <param name="tableNames">需要影射的表名</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, SqlTransaction transaction)
        {
            FillDataSet(commandType, commandText, dataSet, tableNames, null, transaction);
        }

        /// <summary>
        /// 利用TableMappings给DataSet绑定多个表，并给DataSet做相应的表名影射(私有方法)
        /// </summary>
        /// <remarks>
        ///  FillDataset(conn, trans, CommandType.StoredProcedure, "GetOrders", ds, new string[] {"orders"}, new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="dataSet">拥有多个结果集的Dataset</param>
        /// <param name="tableNames">需要影射的表名</param>
        /// <param name="commandParameters">参数数组</param>
        /// <param name="transaction">一个有效的SqlTransaction</param>
        public void FillDataSet(CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, SqlParameter[] commandParameters, SqlTransaction transaction)
        {
            if (connection == null)
            {
                return;
            }
            if (dataSet == null) throw new ArgumentNullException("dataSet");

            bool mustCloseConnection = false;

            using (SqlCommand command = new SqlCommand())
            {
                PrepareCommand(command, transaction, commandType, commandText, commandParameters, out mustCloseConnection);

                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(command))
                {

                    if (tableNames != null && tableNames.Length > 0)
                    {
                        string tableName = "Table";
                        for (int index = 0; index < tableNames.Length; index++)
                        {
                            if (tableNames[index] == null || tableNames[index].Length == 0)
                                throw new ArgumentException(
                                    "The tableNames parameter must contain a list of tables, a value was provided as null or empty string.",
                                    "tableNames");
                            dataAdapter.TableMappings.Add(tableName, tableNames[index]);
                            tableName += (index + 1).ToString();
                        }
                    }

                    dataAdapter.Fill(dataSet);

                    command.Parameters.Clear();
                }
            }
            if (mustCloseConnection)
                connection.Close();
        }

        /// <summary>
        /// 查询出指定页数的数据，返回DataSet
        /// </summary>
        /// <param name="commandType">CommandType类型 (stored procedure, text, etc.)</param>
        /// <param name="commandText">存储过程名称或T-SQL语句</param>
        /// <param name="pageSize">每页数量</param>
        /// <param name="currentPageIndex">当前页</param>
        /// <param name="commandParameters">参数SqlParameter</param>
        /// <returns>DataSet数据集</returns>
        public DataSet ExecutePageDataSet(CommandType commandType, string commandText, int pageSize, int currentPageIndex, SqlParameter[] commandParameters)
        {
            if (connection == null)
            {
                return null;
            }
            using (SqlCommand cmd = new SqlCommand())
            {
                bool mustCloseConnection = false;
                PrepareCommand(cmd, (SqlTransaction)null, commandType, commandText, commandParameters,
                               out mustCloseConnection);

                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    using (DataSet ds = new DataSet())
                    {
                        ds.Locale = CultureInfo.InvariantCulture;
                        if (currentPageIndex < 1)
                        {
                            currentPageIndex = 1;
                        }
                        int startRecord = pageSize * (currentPageIndex - 1);
                        int maxRecords = pageSize;
                        da.Fill(ds, startRecord, maxRecords, "currentPage");

                        cmd.Parameters.Clear();

                        if (mustCloseConnection)
                            connection.Close();

                        return ds;
                    }
                }
            }
        }



        /// <summary>
        /// 如果连接未关闭，则关闭连接  
        /// </summary>
        /// /// <param name="disposing">判断参数</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connection.State != ConnectionState.Closed)
                {
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 重写Dispose  
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        /// <summary>
        /// 枚举表示该连接不论是调用者还是SqlHelper,当执行ExecuteReader()时需要用CommandBehavior
        /// </summary>
        private enum SqlConnectionOwnership
        {
            /// <summary>连接由SqlHelper管理和使用</summary>
            Internal,

            /// <summary>连接由调用者管理和使用</summary>
            External
        }
    }
}