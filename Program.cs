
using System.Xml;
using System.Xml.Schema;


namespace chkschema
{
    class Program 
    {

        public static bool SuccessfulValidation = true;
        public const string XMLNamespace = @"http://csrc.nist.gov/ns/oscal/1.0";
        public const string OSCALVersion = "1.0.4";

        enum ExitCode : int {
            Success = 0,
            Error = -1,
            BadArgs = -2

        }
        static int Main(string[] args)
        {
            int a = 0;
            string Usage = "chkschema Version 1.0 (01-05-2023)";
            Usage = Usage + "\nUsage 1.1: chkschema <OSCAL Document (XML)> <schema.xsd>";
            Usage = Usage + string.Format ("\nCurrent Schemas ({0}):\n\toscal_ssp_schema.xsd (SSP)\n\toscal_assessment-plan_schema.xsd (AP)\n\toscal_assessment-results_schema.xsd (AR)\n\toscal_poam_schema.xsd (POAM)\n\toscal_catalog_schema.xsd (CATALOG)\n\toscal_profile_schema.xsd (PROFILE)\n", OSCALVersion);
            foreach(string arg in args)
            {
                a++;
            }
            if (a == 2) 
            {
                string myDoc = args[0];
                if (!File.Exists(myDoc)) 
                {
                    Console.WriteLine(string.Format("Error: File Not Found - {0}", myDoc));
                    return (int)ExitCode.BadArgs;
                }

                //  Folder Location of Schemas - Put your location in here.
                string myOSCALSchema = string.Format("{0}{1}", "C:\\utils\\schemas\\", args[1]);
                if (!File.Exists(myOSCALSchema))
                {
                    Console.WriteLine(string.Format("Error: File Not Found - {0}", myOSCALSchema));
                    return (int)ExitCode.BadArgs;
                }
                Console.WriteLine("Checking: {0} against {1}...\n", myDoc, myOSCALSchema);
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

                while (OscalDoc.Read()) { }

                OscalDoc.Close();

            }
            catch (Exception)
            {

            }
        }
        private static void OSCALSettingsValidationEventHandler(object? sender, ValidationEventArgs e)
        {
            if (e.Severity == XmlSeverityType.Warning)
            {
                
                Console.WriteLine("Warning: {0}", e.Message);
                SuccessfulValidation = false;

            }
            else if (e.Severity == XmlSeverityType.Error)
            {


                Console.WriteLine("Error: {0}", e.Message);
                SuccessfulValidation = false;

            }
        }
    }
}
