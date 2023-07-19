using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace Facesheet
{
    class ClsExportNew
    {
        clsDataNew objData = null;
        DataTable Dt = null;
        DataTable Dt2 = null;
        string strQuery = "";
        string[] IEPC = null;
        string[] IEPP = null;
        string[] IEPCHK = null;
        string[] Common = null;
        string[] aC = null;
        string[] aP = null;
        int Count = 0, objcnt = 0, RCount = 0;






        public void getIEP1(out string[] C1, out string[] C2, int StudentId, int SchoolId, string[] args, int pageno)
        {
            try
            {
                string Connection = args[1];
                objData = new clsDataNew();
                string type = "";
                Dt = new DataTable();
                Dt2 = new DataTable();
                DataTable dt3 = new DataTable();
                DataTable dt4 = new DataTable();
                IDataReader DR = null;
                int i = 0;
                if (pageno == 1)
                {
                    Count = 26;
                    type = "SM";
                }
                if (pageno == 2)
                {
                    Count = 36;
                    type = "ED";
                }
                if (pageno == 3)
                {
                    Count = 15;
                    type = "SD";
                }
                if (pageno == 4)
                {
                    Count = 21;
                    type = "PP";
                }
                if (pageno == 5)
                {
                    Count = 28;
                    type = "SA";
                }
                try
                {
                    SqlConnection sqlConnection = new SqlConnection(Connection);
                    SqlCommand command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                    command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                    command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                    sqlConnection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter();
                    adapter.SelectCommand = command;
                    adapter.Fill(Dt);
                 //   System.Diagnostics.Debugger.Log(1, "StudentMoreDetailsNE_Client:-Type=" + type, "After SP Call.");
                    sqlConnection.Close();
                    if (pageno == 4)
                    {
                        type = "INEX";
                        sqlConnection = new SqlConnection(Connection);
                        command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                        sqlConnection.Open();
                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(Dt2);
                        sqlConnection.Close();

                        type = "MT";
                        sqlConnection = new SqlConnection(Connection);
                        command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                        sqlConnection.Open();
                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dt3);
                        sqlConnection.Close();

                        

                    }
                    if (pageno == 5)
                    {
                        type = "DD";
                        sqlConnection = new SqlConnection(Connection);
                        command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                        sqlConnection.Open();
                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(Dt2);
                        sqlConnection.Close();

                        type = "DI";
                        sqlConnection = new SqlConnection(Connection);
                        command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                        sqlConnection.Open();
                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dt3);
                        sqlConnection.Close();

                        type = "IEP";
                        sqlConnection = new SqlConnection(Connection);
                        command = new SqlCommand("StudentMoreDetailsNE_Client", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("@SchoolId", SqlDbType.Int).Value = SchoolId;
                        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = StudentId;
                        command.Parameters.Add("@Type", SqlDbType.VarChar).Value = type;
                        sqlConnection.Open();
                        adapter = new SqlDataAdapter();
                        adapter.SelectCommand = command;
                        adapter.Fill(dt4);
                        sqlConnection.Close();
                    }

                }
                catch (SqlException ex)
                {
                    Console.WriteLine("SQL Error" + ex.Message.ToString());

                }

                
                IEPC = new string[Count];
                IEPCHK = new string[Count];

               // System.Diagnostics.Debugger.Log(2, "StudentMoreDetailsNE_Client:-Type=" + type + "IEPC.count=" + IEPC.Length + "IEPCHK.Count=" + IEPCHK.Length, "After SP Call.");
                int index = 0;
                if (pageno == 2)
                {
                    if (Dt.Rows.Count < 4)
                    {
                        int c = 4 - Dt.Rows.Count;
                        for (int h = 0; h < c; h++)
                        {
                            Dt.Rows.Add("", " ", " ", " ", " ", " ", "");
                        }
                    }
                }
              //  System.Diagnostics.Debugger.Log(3, "StudentMoreDetailsNE_Client:-Type=" + type + "Dt.Rows.Count=" + Dt.Rows.Count, "After Dt rows added.");
                if (pageno == 3)
                {
                    if (Dt.Rows.Count < 3)
                    {
                        int c = 3 - Dt.Rows.Count;
                        for (int h = 0; h < c; h++)
                        {
                            Dt.Rows.Add(" ", " ");//, " ");
                        }
                    }
                }
                if (pageno == 5)
                {
                    if (Dt.Rows.Count < 3)
                    {
                        int c = 5 - Dt.Rows.Count;
                        for (int h = 0; h < c; h++)
                        {
                            Dt.Rows.Add(" ", " ", " ");
                        }
                    }
                }
                if (Dt != null)
                {
                    if (Dt.Rows.Count > 0)
                    {

                        foreach (DataRow Dr in Dt.Rows)
                        {
                            for (int j = 0; j < Dt.Columns.Count; j++)
                            {
                                IEPC[i] = Dr[j].ToString();
                                if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                {
                                    index++;
                                    IEPCHK[j] = Dr[j].ToString();
                                }
                                i++;
                            }
                        }

                    }
                    else
                    {
                        i += 3;
                    }
                }
           //     System.Diagnostics.Debugger.Log(4, "StudentMoreDetailsNE_Client:-Type=" + type + "IEPC.count=" + IEPC.Length + "IEPCHK.Count=" + IEPCHK.Length, "After DT Loop.");
                if (pageno == 4)
                {
                    if (Dt2 != null)
                    {
                        if (Dt2.Rows.Count > 0)
                        {

                            foreach (DataRow Dr in Dt2.Rows)
                            {
                                for (int j = 0; j < Dt2.Columns.Count; j++)
                                {
                                    IEPC[i] = Dr[j].ToString();
                                    if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                    {
                                        index++;
                                        IEPCHK[i] = Dr[j].ToString();
                                    }
                                    i++;
                                }
                                
                            }
                            if (Dt2.Rows.Count == 1)
                                i += 3;

                        }
                    }
                    if (dt3 != null)
                    {
                        if (dt3.Rows.Count > 0)
                        {

                            foreach (DataRow Dr in dt3.Rows)
                            {
                                for (int j = 0; j < dt3.Columns.Count; j++)
                                {
                                    IEPC[i] = Dr[j].ToString();
                                    if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                    {
                                        index++;
                                        IEPCHK[i] = Dr[j].ToString();
                                    }
                                    i++;
                                }
                                
                            }

                        }
                    }
                    
                }
                if (pageno == 5)
                {
                    if (Dt2 != null)
                    {
                        if (Dt2.Rows.Count > 0)
                        {

                            foreach (DataRow Dr in Dt2.Rows)
                            {
                                for (int j = 0; j < Dt2.Columns.Count; j++)
                                {
                                    IEPC[i] = Dr[j].ToString();
                                    if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                    {
                                        index++;
                                        IEPCHK[i] = Dr[j].ToString();
                                    }
                                    i++;
                                }
                                
                            }

                        }
                    }
                    if (dt3 != null)
                    {
                        if (dt3.Rows.Count > 0)
                        {

                            foreach (DataRow Dr in dt3.Rows)
                            {
                                for (int j = 0; j < dt3.Columns.Count; j++)
                                {
                                    IEPC[i] = Dr[j].ToString();
                                    if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                    {
                                        index++;
                                        IEPCHK[i] = Dr[j].ToString();
                                    }
                                    i++;
                                }
                                
                            }

                        }
                    }
                    if (dt4 != null)
                    {
                        if (dt4.Rows.Count > 0)
                        {

                            foreach (DataRow Dr in dt4.Rows)
                            {
                                for (int j = 0; j < dt4.Columns.Count; j++)
                                {
                                    IEPC[i] = Dr[j].ToString();
                                    if ((Dr[j].ToString() == "true") || (Dr[j].ToString() == "false"))
                                    {
                                        index++;
                                        IEPCHK[i] = Dr[j].ToString();
                                    }
                                    i++;
                                }

                            }

                        }
                    }
                }
                for (i = 0; i < IEPC.Length; i++)
                {
                    if (IEPC[i] == null)
                    {
                        IEPC[i] = " ";
                    }
                    //if (pageno == 1&&i==22)
                    //{
                    //    IEPC[i] = args[8];
                    //}
                }

            }
            catch (Exception Ex)
            {
                ClsErrorLogNew errlog = new ClsErrorLogNew();
                errlog.WriteToLog(Ex.ToString());
            }


            C1 = new string[Count];
            C2 = new string[Count];
            if (IEPC != null) Array.Copy(IEPC, C1, Count);
            if (IEPC != null) Array.Copy(IEPCHK, C2, Count);

        }




    }
}
