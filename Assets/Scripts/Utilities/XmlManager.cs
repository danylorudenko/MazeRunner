using System.IO;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public static class XmlManager
{
    public static bool lastDataWasRead = false;

    private static string fileName = "recordsData.xml";

    private static string rootElementName = "RecrodsList";
    private static string userNameAttributeName = "userName";
    private static string coinCountAttributeName = "coinCount";
    private static string entryElementName = "RecordEntry";

    public static void TryCreateXmlRecrodsFile()
    {
        if (!File.Exists(fileName)) {
            XmlDocument xmlDocument = new XmlDocument();

            XmlDeclaration declaration = xmlDocument.CreateXmlDeclaration("1.0", "utf-8", null);
            xmlDocument.AppendChild(declaration);

            XmlElement root = xmlDocument.CreateElement(rootElementName);
            xmlDocument.AppendChild(root);

            xmlDocument.Save(fileName);
        }
    }

    public static List<RecordEntry> ReadAllRecords()
    {

        List<RecordEntry> allEntries = new List<RecordEntry>();
        if (!File.Exists(fileName)) {
            return allEntries;
        }

        FileStream xmlFileStream = new FileStream(fileName, FileMode.Open);
        XmlReader reader = XmlReader.Create(xmlFileStream);
        
        while (reader.Read()) {
            if (reader.Name == entryElementName) {
                int coinCout;
                int.TryParse(reader.GetAttribute(coinCountAttributeName), out coinCout);

                string userName = reader.GetAttribute(userNameAttributeName);
                
                allEntries.Add(new RecordEntry(userName, coinCout));
            }
        }

        reader.Close();
        xmlFileStream.Close();

        lastDataWasRead = true;
        return allEntries;
    }

    public static void WriteNewRecord(string userName, int collectedCoins)
    {
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.Load(fileName);

        XmlElement newEntry = xmlDocument.CreateElement(entryElementName);
        newEntry.SetAttribute(coinCountAttributeName, collectedCoins.ToString());
        newEntry.SetAttribute(userNameAttributeName, userName);

        xmlDocument.DocumentElement.AppendChild(newEntry);
        xmlDocument.Save(fileName);

        lastDataWasRead = false;
    }
}