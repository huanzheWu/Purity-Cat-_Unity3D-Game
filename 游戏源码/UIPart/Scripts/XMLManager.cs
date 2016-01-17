using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Xml;

public enum XMLTYPE{Level};

public class XMLManager{

    private string[] g_Level;

    public XMLManager() 
    {
        g_Level = new string[] {"Level", "Open","Pass", "StarNum" };//Level用城市代号_关卡号，Open，Pass用true,false字符串，StarNum用0-3。
    }

    //创建XML文件
    public bool CreateXML(string FileName)
    {
        if (isXMLExict(FileName))
            return false;
        XmlDocument xmlDoc = new XmlDocument();
        //添加XML第一行格式
        XmlDeclaration xmlDec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
        xmlDoc.AppendChild(xmlDec);
        //添加根节点,以文件名作为根节点名
        XmlElement xmlelem = xmlDoc.CreateElement(FileName);
        xmlDoc.AppendChild(xmlelem);
        xmlDoc.Save(GetPathWithFileName(FileName));
        return true;
    }

    //添加节点
    public bool InsertXML(string FileName,XMLTYPE Type,string[] InsertContent)
    {
        string[] PartName = GetPartFromType(Type);
        if (PartName.Length != InsertContent.Length)
            return false;
        if (isXMLExict(FileName))
            CreateXML(FileName);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GetPathWithFileName(FileName));
        XmlNode root = xmlDoc.SelectSingleNode(FileName);
        XmlElement xe_parent = xmlDoc.CreateElement("Parent");
        for (int i = 0; i < PartName.Length; i++)
            xe_parent.SetAttribute(PartName[i], InsertContent[i]);
        root.AppendChild(xe_parent);//添加节点到根节点RootNode中
        xmlDoc.Save(GetPathWithFileName(FileName));
        return true;

    }

    //读取节点,并将所有节点信息存放于OutPutList。
    public bool LoadXML(string FileName,XMLTYPE Type,out ArrayList OutPutList)
    {
        OutPutList = new ArrayList();
        string[] PartName = GetPartFromType(Type);
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GetPathWithFileName(FileName));
        XmlElement rootElem = xmlDoc.DocumentElement;
        XmlNodeList bookNodes = rootElem.GetElementsByTagName("Parent");
        foreach (XmlNode node in bookNodes)
        {
            ArrayList Temp = new ArrayList();
            for (int i = 0; i < PartName.Length; i++)
            {
                string strName = ((XmlElement)node).GetAttribute(PartName[i]);
                Temp.Add(strName);
            }
            OutPutList.Add(Temp);
        }
        return true;
    }

    //在通过关卡后调用ExchangePassLevelXML(Application.loadedLevelName,(获得的星星数))即可完成改变星星且打开下一关
    public void ExchangePassLevelXML(string Level, int StarNum)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GetPathWithFileName("Level"));
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Level").ChildNodes;
        foreach (XmlNode xn in nodeList)
        {
            XmlElement xmlDoc2 = (XmlElement)xn;
            if (xmlDoc2.GetAttribute("Level") == Level)
            {
                xmlDoc2.SetAttribute("Pass" , "true");
                if((int)(xmlDoc2.GetAttribute("StarNum")[0] - '0') < StarNum)
                    xmlDoc2.SetAttribute("StarNum", StarNum.ToString());
                string Temp;
                if (GetNextLevel(Level, out Temp))
                    nodeList = ExchangeOpenLevelXML(nodeList, Temp);
                break;
            }
        }
        xmlDoc.Save(GetPathWithFileName("Level"));
    }

    private XmlNodeList ExchangeOpenLevelXML(XmlNodeList nodeList,string Level)
    {
        foreach (XmlNode xn in nodeList)
        {
            XmlElement xmlDoc2 = (XmlElement)xn;
            if (xmlDoc2.GetAttribute("Level") == Level)
            {
                xmlDoc2.SetAttribute("Open", "true");
                break;
            }
        }
        return nodeList;
    }

    //删除XML节点内容
    public bool DeleteXML(string FileName,string PartName,string DeleteName)
    {
        if (!isXMLExict(FileName))
            return false;
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(GetPathWithFileName(FileName));
        XmlNodeList nodeList = xmlDoc.SelectSingleNode(FileName).ChildNodes;
        foreach (XmlNode xn in nodeList)
        {
            XmlElement xmlDoc2 = (XmlElement)xn;

            if (xmlDoc2.GetAttribute(PartName) == DeleteName)
                xmlDoc2.RemoveAll();
        }
        xmlDoc.Save(GetPathWithFileName(FileName));
        return true;
    }

    //判断文件是否存在
    private bool isXMLExict(string FileName)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(GetPathWithFileName(FileName));
        }
        catch
        {
            return false;
        }
        return true;
    }

    //用文件名来获取路径
    private string GetPathWithFileName(string FileName)
    {
        return Application.persistentDataPath + FileName + ".xml";
    }

    //用XML类型来获得类型由几部分组成
    private string[] GetPartFromType(XMLTYPE Type)
    {
        if (Type == XMLTYPE.Level)
            return g_Level;
        return null;
    }

    private bool GetNextLevel(string Level, out string NextLevel)
    {
        char city = Level[0], level = Level[Level.Length - 1];
        if (level != '8')
            level = (char)((int)level + 1);
        else if (city != 4)
        {
            city = (char)((int)city + 1);
            level = '1';
        }
        else
        {
            NextLevel = null;
            return false;
        }
        NextLevel = city + "_" + level;
        return true;
    }

    private void LevelXMLInit()
    {
        if (!isXMLExict("Level"))
        {
            CreateXML("Level");
            for (int i = 1; i < 5; i++)
            {
                for (int j = 1; j < 9; j++)
                {
                    string[] temp = new string[] { i + "_" + j, "false", "false", "0" };
                    if (i == 1 && j == 1)
                        temp[1] = "true";

                    InsertXML("Level", XMLTYPE.Level, temp);
                }
            }
        }
    }

    private void XMLInit(string FileName)
    {
        if (!isXMLExict(FileName))
            CreateXML(FileName);
    }


    public void InitAllXML()
    {
        LevelXMLInit();
    }
}
