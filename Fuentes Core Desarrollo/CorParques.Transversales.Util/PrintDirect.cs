using CorParques.Transversales.Util;
using System;
using System.Configuration;
using System.Runtime.InteropServices;

[StructLayout(LayoutKind.Sequential)]
public partial struct DOCINFO
{
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDocName;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pOutputFile;
    [MarshalAs(UnmanagedType.LPWStr)]
    public string pDataType;
} // DOCINFO

public partial class PrintDirect
{
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
    public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
    public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long StartPagePrinter(IntPtr hPrinter);
    [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long EndPagePrinter(IntPtr hPrinter);
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long EndDocPrinter(IntPtr hPrinter);
    [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
    public static extern long ClosePrinter(IntPtr hPrinter);

    public static string PrintText(string Text, double codigo)
    {
        string str1 = "";
        string str3 = "";
        string pPrinterName = ConfigurationManager.AppSettings["ImpresoraBrazalete"].ToString();
        try
        {
            IntPtr phPrinter = new IntPtr();
            DOCINFO pDocInfo = new DOCINFO();
            int pcWritten = 0;
            str1 = "Error -1";
            pDocInfo.pDocName = "Direct Text Print";
            pDocInfo.pDataType = "RAW";
            str1 = "Error 0";
            PrintDirect.OpenPrinter(pPrinterName, ref phPrinter, 0);
            str1 = "Error 0 respuesta " + phPrinter.ToString();
            long.Parse(phPrinter.ToString().Trim());
            str1 = "Error 1";
            PrintDirect.StartDocPrinter(phPrinter, 1, ref pDocInfo);
            str1 = "Error 2";
            PrintDirect.StartPagePrinter(phPrinter);
            str1 = "Error 3";
            PrintDirect.WritePrinter(phPrinter, Text, Text.Length, ref pcWritten);
            str1 = "Error 4";
            PrintDirect.EndPagePrinter(phPrinter);
            str1 = "Error 5";
            PrintDirect.EndDocPrinter(phPrinter);
            str1 = "Error 6";
            PrintDirect.ClosePrinter(phPrinter);
            string str2 = pPrinterName + str1;
            //return pPrinterName;
        }
        catch (Exception ex)
        {
            str3 = "";
            return ex.Message + " " + str1;
        }
        return str3;
    }
} // PrintDirect
