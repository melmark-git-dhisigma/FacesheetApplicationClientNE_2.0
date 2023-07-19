using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Configuration;
using System.Text.RegularExpressions;
using System.IO;
using System.Reflection;


/// <summary>
/// Summary description for clsData
/// </summary>
public class clsDataNew
{
    public clsDataNew()
    {

    }


    private string mConectionString = "";
    SqlCommand cmd = null;
    SqlDataAdapter DAdap = null;

    public static bool blnTrans = true;

    public void Reset()
    {
        cmd = null;
        DAdap = null;
    }

    //public byte[] FetchPhoto(string SQL)
    //{
    //    byte[] content;
    //    using (cmd = new SqlCommand())
    //    {
    //        SqlConnection con = Open();
    //        if (blnTrans) cmd.Transaction = Trans;
    //        cmd.CommandText = SQL;
    //        cmd.Connection = con;
    //        content = (byte[])cmd.ExecuteScalar();
    //    }
    //    return content;
    //}

    public void ExecuteSp()
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            // if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "SessionScore_Calculation";

            object content = cmd.ExecuteScalar();
            Close(con);
        }
    }

    public string ExecuteDateIntervalExist(string StartTime, string EndTime, string StartDate, string EndDate, int schoolId, int ClassId, int NumOfTime)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            //  if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "DateIntervalExist";
            cmd.Parameters.AddWithValue("@StartTime", StartTime);
            cmd.Parameters.AddWithValue("@EndTime", EndTime);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@schoolId", schoolId);
            cmd.Parameters.AddWithValue("@ClassId", ClassId);
            cmd.Parameters.AddWithValue("@NumOfTime", NumOfTime);

            string content = cmd.ExecuteScalar().ToString();
            Close(con);
            return content;
        }
    }

    public int SaveLessonPlanData(string name, string contentType, byte[] Data)
    {
        int index = 0;
        using (cmd = new SqlCommand())
        {
            string query = "insert into tbl_Files values (@Name, @ContentType, @Data)";
            SqlConnection con = Open();
            cmd.Connection = con;
            cmd.CommandText = query;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@ContentType", contentType);
            cmd.Parameters.AddWithValue("@Data", Data);

            index = (int)cmd.ExecuteNonQuery();

            Close(con);
            return index;
        }
    }


    public int DownloadLpData(int DocId, out string fileName, out string contentType, out byte[] bytes)
    {
        int index = 0;
        //byte[] bytes = null;
        //string fileName = "";
        //string contentType = "";
        string selQuerry = "";

        using (cmd = new SqlCommand())
        {
            string querry = "select Name, Data, ContentType from tbl_Files where DocId=@DocId";

            SqlConnection con = Open();
            cmd.Connection = con;
            cmd.CommandText = querry;
            cmd.Parameters.AddWithValue("@DocId", DocId);

            using (SqlDataReader sdr = cmd.ExecuteReader())
            {
                sdr.Read();
                bytes = (byte[])sdr["Data"];
                contentType = sdr["ContentType"].ToString();
                fileName = sdr["Name"].ToString();
            }

            Close(con);
        }

        return 1;
    }


    public object ExecuteIOAPercCalculation(int NormalTable, int IOATable)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            // if (blnTrans) cmd.Transaction = Trans;
            cmd.Connection = con;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@NormalSessHdr", NormalTable);
            cmd.Parameters.AddWithValue("@IOASessHdr", IOATable);
            cmd.CommandText = "IOAPercentage_Calculation";

            object content = cmd.ExecuteScalar();
            Close(con);
            return content;
        }
    }

    public void ExecutePhoto(byte[] content, int Id, bool val)
    {
        using (cmd = new SqlCommand())
        {
            SqlConnection con = Open();
            //   if (blnTrans) cmd.Transaction = Trans;
            try
            {

                cmd.Connection = con;
                cmd.CommandType = CommandType.StoredProcedure;

                if (val == true)
                {
                    cmd.CommandText = "saveImage";
                }
                else
                {
                    cmd.CommandText = "updateImage";
                }
                cmd.Parameters.Add("@SMS_AdmReg_ID", SqlDbType.Int);
                cmd.Parameters.Add("@Photo", SqlDbType.Image);
                cmd.Parameters["@SMS_AdmReg_ID"].Value = Id;
                cmd.Parameters["@Photo"].Value = content;
                cmd.ExecuteNonQuery();
                Close(con);
            }
            catch (SqlException ex)
            {
                Close(con);
                if (ex.Message.Contains("Cannot insert duplicate"))
                    throw new Exception("Duplicate");
                ClsErrorLogNew errlog = new ClsErrorLogNew();
                errlog.WriteToLog(ex.ToString());
            }
        }
    }

    public void Dispose()
    {
        mConectionString = "";
        cmd = null;
        DAdap = null;
        //  Trans = null;
        blnTrans = false;
    }

    public string ConnectionString
    {
        get
        {
            return mConectionString = "";// ConfigurationManager.ConnectionStrings["dbConnectionString"].ToString();
        }
    }
    //Get One Value From Table........
    public object FetchValue(string SQL)
    {
        object x = null;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();

            cmd = null;
            using (cmd = new SqlCommand())
            {
                cmd.CommandText = SQL;
                cmd.Connection = con;

                x = cmd.ExecuteScalar();
            }
            Close(con);
        }
        catch (Exception exp)
        {
            Close(con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(exp.ToString());
        }

        return x;
    }
    public object FetchValueTrans(string SQL, SqlTransaction Transs, SqlConnection Con)
    {
        object x = null;

        try
        {
            cmd = null;
            using (cmd = new SqlCommand())
            {
                cmd.Transaction = Transs;
                cmd.CommandText = SQL;
                cmd.Connection = Con;

                x = cmd.ExecuteScalar();
            }

        }
        catch (Exception Ex)
        {
            Close(Con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return x;
    }

    public DataTable ReturnDataTableWithTransaction(string Query, SqlConnection con, SqlTransaction Trans, bool sql)
    {
        DataTable Dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Transaction = Trans;
            DAdap = new SqlDataAdapter(cmd);
            DAdap.Fill(Dt);
            cmd = null;

        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return Dt;
    }


    public DataTable ReturnDataTableDropDown(string Query, bool sql)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            DAdap = new SqlDataAdapter(cmd);
            DAdap.Fill(Dt);
            cmd = null;

        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return Dt;
    }
    public object GetCurrentAutoIncID()
    {
        object x = null;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (SqlCommand cmd = new SqlCommand("SELECT SCOPE_IDENTITY()", con))
            {
                //  if (blnTrans) cmd.Transaction = Trans;
                x = cmd.ExecuteScalar();
            }
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return x;
    }

    public bool IFExists(string SQL)
    {
        bool returnvalue = false;
        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (cmd = new SqlCommand(SQL, con))
            {
                using (SqlDataReader rd = cmd.ExecuteReader())
                {
                    if (rd != null) if (rd.Read()) returnvalue = true;
                    rd.Close();
                }
            }
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return returnvalue;

    }



    public bool IFExistsWithTranss(string SQL, SqlTransaction Trans, SqlConnection con)
    {
        bool returnvalue = false;
        try
        {


            using (cmd = new SqlCommand(SQL, con))
            {
                cmd.Transaction = Trans;
                try
                {
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read()) returnvalue = true;
                        rd.Close();
                    }
                }
                catch (SqlException ex)
                {
                    RollBackTransation(Trans, con);
                    Close(con);
                    ClsErrorLogNew errlog = new ClsErrorLogNew();
                    errlog.WriteToLog(ex.ToString());
                }

                //  Close();

            }
        }
        catch (Exception Ex)
        {
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }

        return returnvalue;


    }

    public int Save(ref DataTable dtAdd)
    {
        int returnValue = 0;
        SqlCommand cmd = null;
        DataTable Dt = new DataTable();

        SqlConnection con = new SqlConnection();
        try
        {
            con = Open();
            using (cmd = new SqlCommand("SELECT * FROM " + dtAdd.TableName + " WHERE 1=2", con))
            {
                //  if (blnTrans) cmd.Transaction = Trans;
                using (DAdap = new SqlDataAdapter(cmd))
                {
                    DAdap.Fill(Dt);
                    DAdap.FillSchema(Dt, SchemaType.Source);

                    DataRow Dr;

                    foreach (DataRow D in dtAdd.Rows)
                    {
                        Dr = Dt.NewRow();
                        Dr.ItemArray = D.ItemArray;
                        Dt.Rows.Add(Dr);
                    }

                    Dt.GetChanges();

                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(DAdap);
                    DAdap.InsertCommand = cmdBuilder.GetInsertCommand();
                    returnValue = DAdap.Update(Dt);
                }
            }
        }
        catch (SqlException Ex)
        {
            Close(con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());

        }
        finally
        {
        }
        Close(con);
        return returnValue;

    }




    public int Update(ref DataTable dtUpd, DataColumn[] primaryKey, string WhereCondition)
    {
        int returnValue = 0;
        DataTable dt = new DataTable();
        SqlConnection con = Open();
        using (cmd = new SqlCommand("SELECT * FROM " + dtUpd.TableName + " WHERE " + WhereCondition, con))
        {
            //if (blnTrans) cmd.Transaction = Trans;
            using (DAdap = new SqlDataAdapter(cmd))
            {
                string pkey = "";
                foreach (DataColumn C in primaryKey)
                {
                    pkey += C.ColumnName + " ";
                }


                DAdap.Fill(dt);
                DAdap.FillSchema(dt, SchemaType.Source);
                DataRow Dr;

                foreach (DataRow D in dtUpd.Rows)
                {
                    Dr = dt.NewRow();
                    foreach (DataColumn C in dtUpd.Columns)
                    {
                        if (pkey.Contains(C.ColumnName)) continue;
                        Dr[C.ColumnName] = D[C.ColumnName];
                    }
                    dt.Rows.Add(Dr);
                }

                dt.GetChanges();
                dt.PrimaryKey = primaryKey;

                using (SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(DAdap))
                {
                    DAdap.UpdateCommand = cmdBuilder.GetUpdateCommand();
                }

                returnValue = DAdap.Update(dt);
            }
        }
        return returnValue;
    }


    public SqlConnection Open()
    {
        SqlConnection mCon = new SqlConnection(ConnectionString);

        try
        {

            mCon.Open();


        }
        catch (SqlException eX)
        {
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(eX.ToString());
        }
        return mCon;
    }

    public SqlConnection Open(bool BeginTrans)
    {
        SqlTransaction Trans;
        SqlConnection con = Open();
        try
        {
            blnTrans = BeginTrans;
            if (BeginTrans) Trans = con.BeginTransaction();

        }
        catch (SqlException eX)
        {
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(eX.ToString());
        }
        return con;
    }

    public void Close(SqlConnection con)
    {
        try
        {

            con.Close();
            con.ConnectionString = "";
            con = null;
            cmd = null;
            DAdap = null;


        }
        catch (SqlException eX)
        {
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(eX.ToString());
        }
    }
    public void CommitTransation()
    {

        // if (blnTrans) Trans.Commit();
        blnTrans = false;

    }
    public void CommitTransation(SqlTransaction Transs, SqlConnection con)
    {
        if (Transs != null)
        {
            Transs.Commit();
            blnTrans = false;
            Close(con);
        }
    }
    public void RollBackTransation()
    {
        // Trans.Rollback();
        blnTrans = false;

    }
    public void RollBackTransation(SqlTransaction Transs, SqlConnection con)
    {
        Transs.Rollback();
        blnTrans = false;
        Close(con);

    }


    //Use this For Insertin ,Update and Deleting.......

    public int ExecuteWithScopeandConnection(string sql, SqlConnection con, SqlTransaction Transs)
    {
        int retval = 0;

        try
        {
            sql = sql + "\nSELECT SCOPE_IDENTITY()";
            using (cmd = new SqlCommand(sql, con))
            {
                cmd.Transaction = Transs;
                try
                {
                    cmd.Connection = con;
                    retval = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    RollBackTransation(Transs, con);
                    //Close();
                    ClsErrorLogNew errlog = new ClsErrorLogNew();
                    errlog.WriteToLog(ex.ToString());
                }

                //  Close();

            }
        }
        catch (Exception Ex)
        {
            Close(con);
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return retval;
    }

    public int ExecuteWithScope(string sql)
    {
        int retval = 0;
        SqlConnection con = Open();
        try
        {

            sql = sql + "\nSELECT SCOPE_IDENTITY()";
            using (cmd = new SqlCommand(sql, con))
            {
                try
                {
                    cmd.Connection = con;
                    retval = Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch (SqlException ex)
                {
                    // RollBackTransation(Trans,con);
                    Close(con);
                    ClsErrorLogNew errlog = new ClsErrorLogNew();
                    errlog.WriteToLog(ex.ToString());
                }

                Close(con);

            }
        }
        catch (Exception Ex)
        {
            Close(con);
            retval = -1;
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return retval;
    }
    public int ExecuteWithTrans(string sql, SqlConnection con, SqlTransaction Transs)
    {
        int retval = 0;

        using (cmd = new SqlCommand(sql, con))
        {
            cmd.Transaction = Transs;
            try
            {

                cmd.Connection = con;
                retval = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                RollBackTransation(Transs, con);
                Close(con);
                return retval;
                ClsErrorLogNew errlog = new ClsErrorLogNew();
                errlog.WriteToLog(ex.ToString());
            }
            return retval;
        }
    }
    public int Execute(string sql)
    {
        int retval = 0;
        SqlConnection con = Open();
        using (cmd = new SqlCommand(sql, con))
        {
            // if (blnTrans) cmd.Transaction = Trans;
            try
            {
                cmd.Connection = con;
                retval = cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                Close(con);
                return retval;
                ClsErrorLogNew errlog = new ClsErrorLogNew();
                errlog.WriteToLog(ex.ToString());
            }
            Close(con);
            return retval;
        }
    }

    public DataTable ReturnDataTable(string TableName)
    {
        DataTable Dt;
        SqlConnection con = Open();
        using (cmd = new SqlCommand("SELECT * FROM " + TableName, con))
        {
            // if (blnTrans) cmd.Transaction = Trans;
            using (DAdap = new SqlDataAdapter(cmd))
            {
                Dt = new DataTable();
                DAdap.Fill(Dt);
            }
        }
        return Dt;
    }

    public SqlDataReader ReturnDataReader(string Query, bool sql)
    {
        SqlConnection con = Open();
        SqlDataReader dr = null;
        if (cmd != null) cmd.Dispose();
        cmd = new SqlCommand(Query, con);

        if (dr != null) if (dr.Read()) dr.Close();
        //  if (blnTrans) cmd.Transaction = Trans;
        dr = cmd.ExecuteReader();
        cmd = null;
        //   Close(con);
        return dr;
    }

    public DataTable ReturnDataTable(string Query, bool sql)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {

            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            Da.Fill(Dt);
            cmd = null;
            Da = null;
            Close(con);
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return Dt;
    }
    public DataTable ReturnDataTable(string Query, SqlConnection con, SqlTransaction Trans, bool sql)
    {
        DataTable Dt = new DataTable();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);
            Da.Fill(Dt);
            cmd = null;
            Da = null;
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null;
            ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return Dt;
    }
    public DataTable ReturnDataTable(string Query, bool sql, bool GetSchema)
    {
        DataTable Dt = new DataTable();
        SqlConnection con = Open();
        try
        {
            SqlCommand cmd = new SqlCommand(Query, con);
            //    if (blnTrans) cmd.Transaction = Trans;
            SqlDataAdapter Da = new SqlDataAdapter(cmd);

            Da.Fill(Dt);
            if (GetSchema)
            {
                try
                {
                    Da.FillSchema(Dt, SchemaType.Source);
                }
                catch (Exception e)
                {
                    if (e.Message == "")
                    {
                        ClsErrorLogNew errlog = new ClsErrorLogNew();
                        errlog.WriteToLog(e.ToString());
                    }
                    Dt = null;

                }
            }
            cmd = null;
            Da = null;
        }
        catch (Exception Ex)
        {
            Close(con);
            Dt = null; ClsErrorLogNew errlog = new ClsErrorLogNew();
            errlog.WriteToLog(Ex.ToString());
        }
        return Dt;
    }


}