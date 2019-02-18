﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace CommonAPI.Common
{
    /// <summary>
    /// A simple helper class for build SQL statements
    /// </summary>
    public class SqlParamHelper
    {
        /// <summary>
        /// Default, empty constructor
        /// </summary>
        public SqlParamHelper()
        {
        }

        /// <summary>
        /// Construct an SQL statement with the supplied SQL and arguments
        /// </summary>
        /// <param name="sql">The SQL statement or fragment</param>
        /// <param name="args">Arguments to any parameters embedded in the SQL</param>
        public SqlParamHelper(string sql, params object[] args)
        {
            _sql = sql;
            _args = args;
        }

        /// <summary>
        /// Instantiate a new SQL Builder object.  Weirdly implemented as a property but makes
        /// for more elegantly readble fluent style construction of SQL Statements
        /// eg: db.Query(Sql.Builder.Append(....))
        /// </summary>
        public static SqlParamHelper Builder
        {
            get { return new SqlParamHelper(); }
        }

        string _sql;
        object[] _args;
        SqlParamHelper _rhs;
        string _sqlFinal;
        object[] _argsFinal;

        private void Build()
        {
            // already built?
            if (_sqlFinal != null)
                return;

            // Build it
            var sb = new StringBuilder();
            var args = new List<object>();
            Build(sb, args, null);
            _sqlFinal = sb.ToString();
            _argsFinal = args.ToArray();
        }

        /// <summary>
        /// Returns the final SQL statement represented by this builder
        /// </summary>
        public string SQL
        {
            get
            {
                Build();
                return _sqlFinal;
            }
        }

        /// <summary>
        /// Gets the complete, final set of arguments collected by this builder.
        /// </summary>
        public SqlParameter[] Arguments
        {
            get
            {
                Build();
                var list = new List<SqlParameter>();
                foreach (var item in _argsFinal)
                {
                    var p = new SqlParameter();
                    p.ParameterName = string.Format("@{0}", list.Count);
                    if (item == null)
                    {
                        p.Value = DBNull.Value;
                    }
                    else
                    {
                        var t = item.GetType();
                        if (t == typeof(bool))
                        {
                            p.Value = ((bool)item) ? 1 : 0;
                        }
                        else if (t.IsEnum)
                        {
                            p.Value = (int)item;
                        }
                        else if (t == typeof(Guid))
                        {
                            p.Value = item.ToString();
                            p.DbType = DbType.String;
                            p.Size = 40;
                        }
                        else if (t == typeof(string))
                        {
                            if ((item as string).Length + 1 > 4000 && p.GetType().Name == "SqlCeParameter")
                                p.GetType().GetProperty("SqlDbType").SetValue(p, SqlDbType.NText, null);

                            p.Size = Math.Max((item as string).Length + 1, 4000);
                            p.Value = item;
                        }
                        else if (item.GetType().Name == "SqlGeography")
                        {
                            p.GetType().GetProperty("UdtTypeName").SetValue(p, "geography", null);
                            p.Value = item;
                        }

                        else if (item.GetType().Name == "SqlGeometry")
                        {
                            p.GetType().GetProperty("UdtTypeName").SetValue(p, "geometry", null);
                            p.Value = item;
                        }
                        else
                        {
                            p.Value = item;
                        }
                    }
                    list.Add(p);
                }
                return list.ToArray();
            }
        }

        /// <summary>
        /// Append another SQL builder instance to the right-hand-side of this SQL builder
        /// </summary>
        /// <param name="sql">A reference to another SQL builder instance</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper Append(SqlParamHelper sql)
        {
            if (_rhs != null)
                _rhs.Append(sql);
            else
                _rhs = sql;

            return this;
        }

        /// <summary>
        /// Append an SQL fragement to the right-hand-side of this SQL builder
        /// </summary>
        /// <param name="sql">The SQL statement or fragment</param>
        /// <param name="args">Arguments to any parameters embedded in the SQL</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper Append(string sql, params object[] args)
        {
            return Append(new SqlParamHelper(sql, args));
        }

        static bool Is(SqlParamHelper sql, string sqltype)
        {
            return sql != null && sql._sql != null && sql._sql.StartsWith(sqltype, StringComparison.InvariantCultureIgnoreCase);
        }

        private void Build(StringBuilder sb, List<object> args, SqlParamHelper lhs)
        {
            if (!String.IsNullOrEmpty(_sql))
            {
                // Add SQL to the string
                if (sb.Length > 0)
                {
                    sb.Append("\n");
                }

                var sql = ParametersHelper.ProcessParams(_sql, _args, args);

                if (Is(lhs, "WHERE ") && Is(this, "WHERE "))
                    sql = "AND " + sql.Substring(6);
                if (Is(lhs, "ORDER BY ") && Is(this, "ORDER BY "))
                    sql = ", " + sql.Substring(9);

                sb.Append(sql);
            }

            // Now do rhs
            if (_rhs != null)
                _rhs.Build(sb, args, this);
        }

        /// <summary>
        /// Appends an SQL WHERE clause to this SQL builder
        /// </summary>
        /// <param name="sql">The condition of the WHERE clause</param>
        /// <param name="args">Arguments to any parameters embedded in the supplied SQL</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper Where(string sql, params object[] args)
        {
            return Append(new SqlParamHelper("WHERE (" + sql + ")", args));
        }

        /// <summary>
        /// Appends an SQL ORDER BY clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of SQL column names to order by</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper OrderBy(params object[] columns)
        {
            return Append(new SqlParamHelper("ORDER BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL SELECT clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of SQL column names to select<param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper Select(params object[] columns)
        {
            return Append(new SqlParamHelper("SELECT " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL FROM clause to this SQL builder
        /// </summary>
        /// <param name="tables">A collection of table names to be used in the FROM clause</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper From(params object[] tables)
        {
            return Append(new SqlParamHelper("FROM " + String.Join(", ", (from x in tables select x.ToString()).ToArray())));
        }

        /// <summary>
        /// Appends an SQL GROUP BY clause to this SQL builder
        /// </summary>
        /// <param name="columns">A collection of column names to be grouped by</param>
        /// <returns>A reference to this builder, allowing for fluent style concatenation</returns>
        public SqlParamHelper GroupBy(params object[] columns)
        {
            return Append(new SqlParamHelper("GROUP BY " + String.Join(", ", (from x in columns select x.ToString()).ToArray())));
        }

        private SqlJoinClause Join(string JoinType, string table)
        {
            return new SqlJoinClause(Append(new SqlParamHelper(JoinType + table)));
        }

        /// <summary>
        /// Appends an SQL INNER JOIN clause to this SQL builder
        /// </summary>
        /// <param name="table">The name of the table to join</param>
        /// <returns>A reference an SqlJoinClause through which the join condition can be specified</returns>
        public SqlJoinClause InnerJoin(string table) { return Join("INNER JOIN ", table); }

        /// <summary>
        /// Appends an SQL LEFT JOIN clause to this SQL builder
        /// </summary>
        /// <param name="table">The name of the table to join</param>
        /// <returns>A reference an SqlJoinClause through which the join condition can be specified</returns>
        public SqlJoinClause LeftJoin(string table) { return Join("LEFT JOIN ", table); }

        /// <summary>
        /// The SqlJoinClause is a simple helper class used in the construction of SQL JOIN statements with the SQL builder
        /// </summary>
        public class SqlJoinClause
        {
            private readonly SqlParamHelper _sql;

            public SqlJoinClause(SqlParamHelper sql)
            {
                _sql = sql;
            }

            /// <summary>
            /// Appends a SQL ON clause after a JOIN statement
            /// </summary>
            /// <param name="onClause">The ON clause to be appended</param>
            /// <param name="args">Arguments to any parameters embedded in the supplied SQL</param>
            /// <returns>A reference to the parent SQL builder, allowing for fluent style concatenation</returns>
            public SqlParamHelper On(string onClause, params object[] args)
            {
                return _sql.Append("ON " + onClause, args);
            }
        }
    }

    internal static class ParametersHelper
    {
        // Helper to handle named parameters from object properties
        public static string ProcessParams(string sql, object[] args_src, List<object> args_dest)
        {
            return rxParams.Replace(sql, m =>
            {
                string param = m.Value.Substring(1);

                object arg_val;

                int paramIndex;
                if (int.TryParse(param, out paramIndex))
                {
                    // Numbered parameter
                    if (paramIndex < 0 || paramIndex >= args_src.Length)
                        throw new ArgumentOutOfRangeException(string.Format("Parameter '@{0}' specified but only {1} parameters supplied (in `{2}`)", paramIndex, args_src.Length, sql));
                    arg_val = args_src[paramIndex];
                }
                else
                {
                    // Look for a property on one of the arguments with this name
                    bool found = false;
                    arg_val = null;
                    foreach (var o in args_src)
                    {
                        var pi = o.GetType().GetProperty(param);
                        if (pi != null)
                        {
                            arg_val = pi.GetValue(o, null);
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                        throw new ArgumentException(string.Format("Parameter '@{0}' specified but none of the passed arguments have a property with this name (in '{1}')", param, sql));
                }

                // Expand collections to parameter lists
                if ((arg_val as System.Collections.IEnumerable) != null &&
                    (arg_val as string) == null &&
                    (arg_val as byte[]) == null)
                {
                    var sb = new StringBuilder();
                    foreach (var i in arg_val as System.Collections.IEnumerable)
                    {
                        sb.Append((sb.Length == 0 ? "@" : ",@") + args_dest.Count.ToString());
                        args_dest.Add(i);
                    }
                    return sb.ToString();
                }
                else
                {
                    args_dest.Add(arg_val);
                    return "@" + (args_dest.Count - 1).ToString();
                }
            }
            );
        }

        static Regex rxParams = new Regex(@"(?<!@)@\w+", RegexOptions.Compiled);
    }

}