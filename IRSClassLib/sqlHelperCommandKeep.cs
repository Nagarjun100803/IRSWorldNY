using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;

namespace IRSClassLib
{
    [Serializable]
    public class sqlHelperCommandKeep
    {
        private bool for_ws;

        public enum KeepFormat
        {
            NONE,
            SQLParams
        }

        private String command_text;
        private int command_timeout;
        private CommandType command_type;
        private KeepFormat keep_format;

        private Object[] al_keep;

        public KeepFormat getKeepFormat()
        {
            return keep_format;
        }

        public Object[] toKeepObject()
        {
            return new Object[] { command_text, command_timeout, command_type, keep_format, al_keep };
        }

        public void setKeepObject(Object[] keep_object)
        {
            command_text = (String) keep_object[0];
            command_timeout = (int) keep_object[1];
            command_type = (CommandType)keep_object[2];
            keep_format = (KeepFormat) keep_object[3];
            al_keep = (Object[])keep_object[4];
        }

        public void setCommandName(String cmd_name)
        {
            this.command_text = cmd_name;
        }

        public void setCommandTimeout(int cmd_to)
        {
            this.command_timeout = cmd_to;
        }

        public void setCommandType(CommandType cmd_type)
        {
            this.command_type = cmd_type;
        }

        public void setValues(Object[] values)
        {
            if (keep_format == KeepFormat.NONE)
            {
                al_keep = new Object[values.Length];
                for (int i = 0; i < values.Length; i++)
                {
                    al_keep[i] = (for_ws) ? replace_dbnull(values[i]) : values[i];
                }
            }
            else
            {
                throw new Exception("setValues requires KeepFormat.NONE");
            }
        }

        public Object[] getValues()
        {
            if (keep_format == KeepFormat.NONE)
            {
                return al_keep;
            }
            else
            {
                Object[] oo = new Object[al_keep.Length];
                for (int i = 0; i < al_keep.Length; i++)
                {
                    Object[] oo_keep = (Object[])al_keep[i];
                    oo[i] = oo_keep[5];
                }
                return oo;
            }
        }

        public void setKeepFormat(KeepFormat kform)
        {
            this.keep_format = kform;
        }

        public void setParameter(String param_name, SqlDbType dbtype, int size, ParameterDirection direction, bool isnullable, Object value)
        {
            ArrayList al = new ArrayList();
            if (al_keep.Length > 0)
            {
                foreach (Object o in al_keep)
                {
                    al.Add(o);
                }
            }

            al.Add(new Object[] { param_name, dbtype, size, direction, isnullable, value });
            al_keep = new Object[al.Count];
            al.CopyTo(al_keep);
        }

        private Object[] create_cmdparms_table(SqlParameterCollection parms)
        {
            if (parms.Count > 0)
            {
                ArrayList al = new ArrayList();
                foreach (SqlParameter p in parms)
                {
                    al.Add(new Object[] { p.ParameterName, p.SqlDbType, p.Size, p.Direction, p.IsNullable, ((for_ws) ? replace_dbnull(p.Value) : p.Value) });
                }

                Object[] oo = new Object[al.Count];
                al.CopyTo(oo);
                return oo;
            }

            return null;
        }

        private void split_cmd(Object o)
        {
            SqlCommand cmd = (SqlCommand)o;
            if (cmd == null || cmd.CommandText == null) return;
            command_text = cmd.CommandText;
            command_timeout = cmd.CommandTimeout;
            command_type = cmd.CommandType;
            keep_format = KeepFormat.SQLParams;
            if (cmd.Parameters.Count > 0)
            {
                al_keep = create_cmdparms_table(cmd.Parameters);
            }
        }

        public void clear()
        {
            al_keep = null;
            command_text = null;
            command_type = CommandType.Text;
            command_timeout = 0;
            keep_format = KeepFormat.SQLParams;
        }

        //[MethodImpl(MethodImplOptions.Synchronized)]
        public void keep(SqlCommand cmd, bool nowait)
        {
            clear();
            if (!nowait)
            {
                split_cmd((Object)cmd);
            }
            else
            {
                new Thread(new ParameterizedThreadStart(split_cmd)).Start(cmd);
            }
        }

        private Object replace_dbnull(Object value)
        {
            object retval = value;

            if (value == DBNull.Value) retval = null;
            else if (value != null && value.GetType() == typeof(System.Data.SqlTypes.SqlGuid)) retval = value.ToString();
            return retval;
        }

        private Object reverse_dbnull(DbType type, Object value)
        {
            Object retval = value;
            if (value == null) retval = DBNull.Value;
            else if (type == DbType.Guid) retval = System.Data.SqlTypes.SqlGuid.Parse(value.ToString().Trim());
            return retval;
        }

        private void recreate_parameters(SqlCommand cmd)
        {
            if (al_keep != null && al_keep.Length > 0)
            {
                foreach (Object o in al_keep)
                {
                    Object[] plist = (Object[])o;
                    SqlParameter p = new SqlParameter
                        (
                            (String)plist[0],
                            (SqlDbType)plist[1],
                            (int)plist[2]
                        );
                    p.Direction = (ParameterDirection)plist[3];
                    p.IsNullable = (bool)plist[4];
                    p.Value = (for_ws & p.Direction != ParameterDirection.ReturnValue) ? reverse_dbnull(p.DbType, plist[5]) : plist[5];

                    cmd.Parameters.Add(p);
                }
            }
        }

        public SqlCommand create_cmd()
        {
            if (command_text != null)
            {
                SqlCommand cmd = new SqlCommand(command_text);
                cmd.CommandType = command_type;
                cmd.CommandTimeout = command_timeout;
                if (keep_format == KeepFormat.SQLParams)
                {
                    recreate_parameters(cmd);
                }

                return cmd;
            }
            return null;
        }

        public sqlHelperCommandKeep() 
        {
            for_ws = false;
        }

        public sqlHelperCommandKeep(bool for_ws)
        {
            this.for_ws = for_ws;
        }
    }
}
