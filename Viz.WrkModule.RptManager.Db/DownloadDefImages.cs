using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Security.Cryptography;
using Devart.Data.Oracle;
using SMBLibrary;
using SMBLibrary.Client;
using Smv.Data.Oracle;
using Viz.DbApp.Psi;


namespace Viz.WrkModule.RptManager.Db
{
  public static class AuthData
  {
    public static string AuthServerNameStore { get; set; }
    public static string AuthShareNameStore { get; set; }
    public static string AuthDomainStore { get; set; }
    public static string AuthUserStore { get; set; }
    public static string AuthPasswordStore { get; set; }
    public static string PathSource { get; set; }
    public static string PathDest { get; set; }
  }
  
  public sealed class DownloadDefImagesRptParam : Smv.Xls.XlsInstanceParam
  {
    public string ListMatLocalNum { get; set; }
    public string ListDef { get; set; }
    public string AgTyp { get; set; }
    public string DestPath { get; set; }
    public DownloadDefImagesRptParam(string sourceXlsFile, string destXlsFile) : base(sourceXlsFile, destXlsFile)
    {}
  }

  public sealed class DownloadDefImages : Smv.Xls.XlsRpt
  {

    protected override void DoWorkXls(object sender, DoWorkEventArgs e)
    {
      var prm = (e.Argument as DownloadDefImagesRptParam);
      
      try{
        this.RunRpt(prm);
      }
      catch (Exception ex){
        Debug.Assert(prm != null, "prm != null");
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка выгрузки", ex.Message, MessageBoxImage.Stop)));
      }
      finally{
        GC.Collect();
      }
    }

    private string GetAuthParam(string stmtAut)
    {
      string rezVal = null;
      
      OracleParameter rezPrm = null;
      var lstPrm = new List<OracleParameter>();

      rezPrm = new OracleParameter
      {
        DbType = DbType.String,
        Direction = ParameterDirection.ReturnValue,
        OracleDbType = OracleDbType.VarChar
      };
      lstPrm.Add(rezPrm);

      Odac.ExecuteNonQuery(stmtAut, CommandType.StoredProcedure, false, lstPrm);
      rezVal = Convert.ToString(rezPrm.Value);
      return rezVal;
    }

    private void FillOutTable(string agTyp, string listMat, string listDef)
    {
      const string sqlStmt = "VIZ_PRN.DWNLD_DEF_IMGS.DownLoadDefImages4ListMat";

      OracleParameter prm = null;
      var lstPrm = new List<OracleParameter>();

      prm = new OracleParameter
      {
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        Value = agTyp
      };
      lstPrm.Add(prm);

      string[] stringSeparators = { "," };
      string[] dlmString = listMat.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

      prm = new OracleParameter
      {
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        ArrayLength = dlmString.Length,
        Value = dlmString
      };
      lstPrm.Add(prm);

      dlmString = listDef.Split(stringSeparators, StringSplitOptions.RemoveEmptyEntries);

      prm = new OracleParameter
      {
        DbType = DbType.String,
        Direction = ParameterDirection.Input,
        OracleDbType = OracleDbType.VarChar,
        ArrayLength = dlmString.Length,
        Value = dlmString
      };
      lstPrm.Add(prm);

      Odac.ExecuteNonQuery(sqlStmt, CommandType.StoredProcedure, false, lstPrm);

    }

    private void CopyImage2()
    {
      var client = new SMB2Client();
      bool isConnected = client.Connect(AuthData.AuthServerNameStore, SMBTransportType.DirectTCPTransport);

      if (isConnected){

        var status = client.Login(AuthData.AuthDomainStore, AuthData.AuthUserStore, AuthData.AuthPasswordStore);

        if (status == NTStatus.STATUS_SUCCESS){

          List<string> shares = client.ListShares(out status);
          ISMBFileStore fileStore = client.TreeConnect(AuthData.AuthShareNameStore, out status);

          /*
          object directoryHandle;
          FileStatus _fileStatus;
          status = fileStore.CreateFile(out directoryHandle, out _fileStatus, @"Work\2023\01\1390683\MATDEFECTBINLINK", AccessMask.GENERIC_READ, SMBLibrary.FileAttributes.Directory, ShareAccess.Read | ShareAccess.Write, CreateDisposition.FILE_OPEN, CreateOptions.FILE_DIRECTORY_FILE, null);
          if (status == NTStatus.STATUS_SUCCESS)
          {
            List<QueryDirectoryFileInformation> fileList;
            status = fileStore.QueryDirectory(out fileList, directoryHandle, "*", FileInformationClass.FileDirectoryInformation);
            status = fileStore.CloseFile(directoryHandle);
          }
          */

          status = fileStore.CreateFile(out var fileHandle, out var fileStatus, AuthData.PathSource,
                            AccessMask.GENERIC_READ | AccessMask.SYNCHRONIZE, SMBLibrary.FileAttributes.Normal, ShareAccess.Read, CreateDisposition.FILE_OPEN, CreateOptions.FILE_NON_DIRECTORY_FILE | CreateOptions.FILE_SYNCHRONOUS_IO_ALERT, null);

          if (status == NTStatus.STATUS_SUCCESS){

            var mStream = new MemoryStream();
            
            long bytesRead = 0;
            byte[] data;

            while (true){

              status = fileStore.ReadFile(out data, fileHandle, bytesRead, (int)client.MaxReadSize);

              if (status != NTStatus.STATUS_SUCCESS && status != NTStatus.STATUS_END_OF_FILE){
                throw new Exception("Failed to read from file");
              }

              if (status == NTStatus.STATUS_END_OF_FILE || data.Length == 0){

                using (var fStream = File.Create(AuthData.PathDest)){
                  mStream.Position = 0;
                  mStream.CopyTo(fStream);
                  mStream.Dispose();
                }
                break;
              }

              bytesRead += data.Length;
              mStream.Write(data, 0, data.Length);
            }

          }

        }

        client.Logoff();
        client.Disconnect();
      }
    }

    private void CopyImage()
    {
      try
      {
        //AuthData.PathSource = AuthData.PathSource.Replace(@"\\", @"\");
        MessageBox.Show(AuthData.PathSource);

        bool validLogin = false;

        using (PrincipalContext tempContext = new PrincipalContext(ContextType.Domain, AuthData.AuthDomainStore, null, ContextOptions.Negotiate))
        {
          try
          {
            validLogin = tempContext.ValidateCredentials(AuthData.AuthUserStore, AuthData.AuthPasswordStore, ContextOptions.Negotiate);
          }
          catch (Exception ex)
          {
            MessageBox.Show(ex.Message);
          }
        }
        if (validLogin)
        {
          //File.Copy(@"C:\folder\filename.txt", @"\\domain\folder\filename.txt", true);
          File.Copy(AuthData.PathSource, AuthData.PathDest, true);

        }
        else
        {
          MessageBox.Show("Username or Password is incorrect...");
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show(ex.Message);
      }
    }

    private Boolean RunRpt(DownloadDefImagesRptParam prm)
    {
      var result = true;

      try
      {
        AuthData.AuthServerNameStore = GetAuthParam("VIZ_PRN.DWNLD_DEF_IMGS.GetAuthServerNameStore");
        AuthData.AuthShareNameStore = GetAuthParam("VIZ_PRN.DWNLD_DEF_IMGS.GetAuthShareNameStore");
        AuthData.AuthDomainStore = GetAuthParam("VIZ_PRN.DWNLD_DEF_IMGS.GetAuthDomainStore");
        AuthData.AuthUserStore = GetAuthParam("VIZ_PRN.DWNLD_DEF_IMGS.GetAuthUserStore");
        AuthData.AuthPasswordStore = GetAuthParam("VIZ_PRN.DWNLD_DEF_IMGS.GetAuthPasswordStore");
        
        FillOutTable(prm.AgTyp, prm.ListMatLocalNum, prm.ListDef);

        var odr = Odac.GetOracleReader("SELECT MATBEZEICHNUNGOUTPUT, BRIG FROM VIZ_PRN.TMP_THICKNESS_2NDCUT", CommandType.Text, false, null, null);

        if (odr != null){

          while (odr.Read())
          {
            AuthData.PathDest = prm.DestPath + '\\' + odr.GetString(0);
            AuthData.PathSource = odr.GetString(1);
            CopyImage2();
          }

          odr.Close();
        }
      }
      catch (Exception ex){
        prm.Disp.Invoke(DispatcherPriority.Normal, (ThreadStart)(() => Smv.Utils.DxInfo.ShowDxBoxInfo("Ошибка выгрузки", ex.Message, MessageBoxImage.Stop)));
        result = false;
      }
      finally{
        result = false;
      }
    
      return result;
    }


  }






}

