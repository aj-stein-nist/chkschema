using System;
using System.Xml.Linq;
// using DocumentFormat.OpenXml;
// using DocumentFormat.OpenXml.Packaging;
// using DocumentFormat.OpenXml.Spreadsheet;  
// using DocumentFormat.OpenXml.Wordprocessing;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Schema;
// using OSCALHelperClasses;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace chkschema
{
    class Program 
    {

       // static string CS = @"Server=localhost\SQLEXPRESS01;Database=CONVERTREPO;Trusted_Connection=False;User Id=vitgadmin; Password=neBUla6853#";
       // static string FR_Profile = "https://raw.githubusercontent.com/GSA/fedramp-automation/release/fedramp1.0.0-oscal1.0.0/baselines/rev4/xml/FedRAMP_rev4_MODERATE-baseline_profile.xml";

        //static string CS = "Server=sqldev19.volpegroup.com;Database=OWT_DEV;Trusted_Connection=False;User Id=vitgadmin; Password=neBUla6853#";
        //static string DNS = "http://csrc.nist.gov/ns/oscal/1.0";
        //static string NS ="https://fedramp.gov/ns/oscal";
        //static string CatREF = "https://doi.org/10.6028/NIST.SP.800-60v2r1";
        //static string SSPTemplateFile = "SSPOUTLINE.txt";
        //static string SystemIdentifierType = "https://fedramp.gov";
        //static string OSCALVer = "1.0.2";
        //static string SSPText = string.Empty;
        //public StreamWriter sw;
        //public static string SecuritySensitivityLevel = string.Empty;
        //List<string> ControlIDs;
        //public static List<string>[,] RespRoleList = new List<string>[10,2];
        //public static int RRListCount = 0;
        //public static int imageCount = 0;
        //public static int mapId = 1;
        public static int val_line=0;
        public static bool SuccessfulValidation = true;
        //static string AppRootPath = "C:\\Users\\volpe\\OneDrive\\Documents\\VS Code\\WordToOSCAL\\";
        public const string XMLNamespace = @"http://csrc.nist.gov/ns/oscal/1.0";
        public const string SSPschema = "oscal_ssp_schema.xsd";
        enum ExitCode : int {
            Success = 0,
            Error = -1,
            BadArgs = -2

        }
        static int Main(string[] args)
        {
            int a = 0;
            string Usage = "Usage 1.0: chkschema <OSCAL Document> <schema.xsd>";
            Usage = Usage + "SSP - oscal_ssp_schema.xsd\nSAP - oscal_assessment-plan_schema.xsd\nSAR - oscal_assessment-results_schema.xsd\nPOAM - oscal_poam_schema.xsd\nCATLOG - oscal_catalog_schema.xsd\nPROFILE - oscal_profile_schema.xsd\n";
            foreach(string arg in args)
            {
                a++;
            }
            if (a == 2) 
            {
                string myDoc = args[0];
                string myOSCALSchema = string.Format("{0}{1}", "C:\\Users\\volpe\\OneDrive\\Desktop\\OSCALDemoFiles\\schemas\\", args[1]);
                Console.WriteLine("Checking: {0} against {1}...", myDoc, myOSCALSchema);
                 PseudoValidator(myDoc, myOSCALSchema);
                 if (!SuccessfulValidation)
                 {  
                    return (int)ExitCode.Error;
                 }
                 else
                    Console.WriteLine("OK.");
                    return (int)ExitCode.Success;
            }
            else 
            {
                Console.WriteLine(Usage);
                return (int)ExitCode.BadArgs;
            }
 
        }

         public static void PseudoValidator(string XmlDocument, string XsdSchemaPath)
        {
            try
            {

                XmlReaderSettings OscalSettings = new XmlReaderSettings();
                OscalSettings.Schemas.Add(XMLNamespace, XsdSchemaPath);
                OscalSettings.ValidationType = ValidationType.Schema;
                OscalSettings.ValidationEventHandler += new ValidationEventHandler(OSCALSettingsValidationEventHandler);
                XmlReader OscalDoc = XmlReader.Create(XmlDocument, OscalSettings);
                val_line=0;
                while (OscalDoc.Read()) { val_line++; }

                OscalDoc.Close();
                //SuccessfulValidation = true;
            }
            catch (Exception ex)
            {
                // SuccessfulValidation = false;
                throw ex;
            }
        }
        private static void OSCALSettingsValidationEventHandler(object sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                
                Console.WriteLine("Warning: {0}", e.Message);
                SuccessfulValidation = false;
                //throw e.Exception;
            }
            else if (e.Severity == XmlSeverityType.Error)
            {


                Console.WriteLine("Error: {0}", e.Message);
                SuccessfulValidation = false;
                //throw e.Exception;
            }
        }
    }
}
