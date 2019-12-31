/*********************************************************************** 
 * 项目名称 :  Peanut.Helper
 * 项目描述 :      
 * 类 名 称 :  XmlHelper 
 * 说    明 :      
 * 作    者 :  XHT  
 * 创建时间 :  2018-10-29 09:25:40 
 * 更新时间 :  2018-11-10 15:21:40 
************************************************************************ 
 * Copyright @   2018. All rights reserved. 
************************************************************************/

using System;
using System.Xml;
using System.Data;
using System.Configuration;

namespace Peanut.Helper.XML
{
    /// <summary>
    /// Xml文档工具类
    /// 
    /// 目的：
    ///     提供基于Dom + xpath方式的操作。包括了：创建节点，节点读取，删除节点，编辑节点等。
    /// 
    ///     xpath格式：
    ///     表达式            描述
    ///     nodename          选取此节点的所有子节点。 
    ///     /                   从根节点选取。 
    ///     //                  从匹配选择的当前节点选择文档中的节点，而不考虑它们的位置。 
    ///     .                   选取当前节点。 
    ///     ..                  选取当前节点的父节点。 
    ///     @                   选取属性。 
    /// 
    /// 使用规范：
    ///     略
    /// </summary>
    public sealed class XmlHelper
    {
        private XmlDocument xmlDoc = default(XmlDocument);   //文档对象
        private readonly string xmlFileName;                 //文件名          
        private readonly string nameSpace;                   //命名空间  
        private readonly bool hasNamespace;                  //标识xml文档是否存在命名空间空间
        private readonly XmlNamespaceManager nsManager;      //命名空间管理对象

        #region 构造函数

        /// <summary>
        /// 创建XmlDomHelper
        /// </summary>
        /// <param name="fileName">Xml文档名。文档路径+文档名</param>
        public XmlHelper(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            xmlFileName = fileName;

            Load(fileName);
        }

        /// <summary>
        /// 创建XmlDomHelper
        /// </summary>
        /// <param name="fileName">Xml文档名。文档路径+文档名</param>
        /// <param name="namespaceString">命名空间。Xml文档中指定的命名空间</param>
        /// <param name="namespacePrefix">命名空间前缀。自定义的命名空间前缀</param>
        public XmlHelper(string fileName, string namespaceString, string namespacePrefix)
        {
            if (string.IsNullOrWhiteSpace(fileName)
                || string.IsNullOrWhiteSpace(namespaceString)
                || string.IsNullOrWhiteSpace(namespacePrefix))
                throw new ArgumentNullException();

            xmlFileName = fileName;
            nameSpace = namespaceString;
            hasNamespace = true;

            Load(fileName);

            nsManager = new XmlNamespaceManager(xmlDoc.NameTable);
            if (!nsManager.HasNamespace(namespacePrefix))
                nsManager.AddNamespace(namespacePrefix, namespaceString);
        }

        #endregion

        #region 属性

        /// <summary>
        /// 完整的Xml文件名
        /// 例如：c:/Xml/Test.xml
        /// </summary>
        public string FileName
        {
            get
            {
                return xmlFileName;
            }
        }

        /// <summary>
        /// Xml文件里定义的命名空间
        /// </summary>
        public string FileNameSpace
        {
            get
            {
                return nameSpace;
            }
        }

        #endregion

        #region 方法

        #region 获取默认路径下的配置文件信息

        /// <summary>
        /// 获取默认路径下的配置文件里指定appSetting的值
        /// </summary>
        /// <param name="appSetting"></param>
        /// <returns></returns>
        public static string GetDefaultAppSettings(string appSetting)
        {
            return ConfigurationManager.AppSettings[appSetting];
        }

        #endregion

        #region 文档加载和保存

        /// <summary>
        /// 加载Xml文档
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private void Load(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            if (xmlDoc == null)
                xmlDoc = new XmlDocument();

            xmlDoc.Load(fileName);
        }

        /// <summary>
        /// 保存Xml文档
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentNullException();

            xmlDoc.Save(fileName);
        }

        #endregion

        #region 新增节点

        /// <summary>
        /// 新增子节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="childNode"></param>
        /// <returns></returns>
        public bool AppendChild(XmlNode parentNode, XmlNode childNode)
        {
            if (parentNode == null || childNode == null)
                throw new ArgumentNullException();

            XmlNode addedNode = parentNode.AppendChild(childNode);

            return addedNode != null;
        }

        /// <summary>
        /// 新增子节点
        /// </summary>
        /// <param name="parentNodeXpath"></param>
        /// <param name="childNode"></param>
        /// <returns></returns>
        public bool AppendChild(string parentNodeXpath, XmlNode childNode)
        {
            if (string.IsNullOrWhiteSpace(parentNodeXpath) || childNode == null)
                throw new ArgumentNullException();

            XmlNode parenNode = xmlDoc.SelectSingleNode(parentNodeXpath);

            if (parenNode == null)
                throw new Exception("ParenNode is Not Exists");

            return AppendChild(parenNode, childNode);
        }

        /// <summary>
        /// 在指定节点前插入节点
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="refNode"></param>
        private void InsertBefore(XmlNode newNode, XmlNode refNode)
        {
            if (newNode == null)
                throw new ArgumentNullException("newNode");

            if (refNode == null)
                throw new ArgumentNullException("refNode");

            xmlDoc.InsertBefore(newNode, refNode);
        }

        /// <summary>
        /// 在指定节点前插入节点
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="refNodeXpath"></param>
        public void InsertBefore(XmlNode newNode, string refNodeXpath)
        {
            if (newNode == null || string.IsNullOrWhiteSpace(refNodeXpath))
                throw new ArgumentNullException();

            XmlNode refNode = xmlDoc.SelectSingleNode(refNodeXpath);

            if (refNode == null)
                throw new Exception("RefNode is null");

            InsertBefore(newNode, refNode);
        }

        /// <summary>
        /// 在指定节点后插入节点
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="refNode"></param>
        public bool InsertAfter(XmlNode newNode, XmlNode refNode)
        {
            if (newNode == null)
                throw new ArgumentNullException("newNode");

            if (refNode == null)
                throw new ArgumentNullException("refNode");

            xmlDoc.InsertBefore(newNode, refNode);

            return true;
        }

        /// <summary>
        /// 在指定节点后插入节点
        /// </summary>
        /// <param name="newNode"></param>
        /// <param name="refNodeXpath"></param>
        public void InsertAfter(XmlNode newNode, string refNodeXpath)
        {
            if (newNode == null || string.IsNullOrWhiteSpace(refNodeXpath))
                throw new ArgumentNullException();

            XmlNode refNode = xmlDoc.SelectSingleNode(refNodeXpath);

            if (refNode == null)
                throw new Exception("RefNode is null");

            InsertAfter(newNode, refNode);
        }

        #endregion

        #region 编辑节点

        /// <summary>
        /// 替换子节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="newNode"></param>
        /// <param name="oldNode"></param>
        public void ReplaceNode(XmlNode parentNode, XmlNode newNode, XmlNode oldNode)
        {
            if (parentNode == null || newNode == null || oldNode == null)
                throw new ArgumentNullException();

            parentNode.ReplaceChild(newNode, oldNode);
        }

        /// <summary>
        /// 设置节点属性的值
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="attrName"></param>
        /// <param name="attrValue"></param>
        public void SetAttributeValue(XmlNode parentNode, string attrName, string attrValue)
        {
            if (parentNode == null)
                throw new ArgumentNullException("parentNode");

            if (string.IsNullOrWhiteSpace(attrName))
                throw new ArgumentNullException("attrName");

            if (string.IsNullOrWhiteSpace(attrValue))
                throw new ArgumentNullException("attrValue");

            if (parentNode.Attributes != null)
                parentNode.Attributes[attrName].Value = attrValue;
        }

        /// <summary>
        /// 设置节点innertext
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="nodeInnerText"></param>
        public void SetInnerText(XmlNode currentNode, string nodeInnerText)
        {
            if (currentNode == null)
                throw new ArgumentNullException("currentNode");

            if (string.IsNullOrWhiteSpace(nodeInnerText))
                throw new ArgumentNullException("nodeInnerText");

            currentNode.InnerText = nodeInnerText;
        }

        /// <summary>
        /// 设置节点值
        /// </summary>
        /// <param name="currentNode"></param>
        /// <param name="nodeValue"></param>
        public void SetValue(XmlNode currentNode, string nodeValue)
        {
            if (currentNode == null)
                throw new ArgumentNullException("currentNode");

            if (string.IsNullOrWhiteSpace(nodeValue))
                throw new ArgumentNullException("nodeValue");

            currentNode.Value = nodeValue;
        }

        #endregion

        #region 删除节点

        /// <summary>
        /// 删除节点
        /// </summary>
        /// <param name="xpath">要删除的节点的路径</param>
        public bool RemoveNode(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                throw new ArgumentNullException("xpath");

            //要删除节点
            XmlNode needRemoveNode = xmlDoc.SelectSingleNode(xpath);

            if (needRemoveNode == null)
                throw new Exception("NeedRemoveNode is null");

            //该节点父节点
            XmlNode parentNode = needRemoveNode.ParentNode;

            //从父节点中移除子节点（如果多个子节点？）
            if (parentNode == null)
                throw new Exception("ParentNode is null");

            parentNode.RemoveChild(needRemoveNode);

            return true;
        }

        /// <summary>
        /// 删除子节点
        /// 同名第一个子节点
        /// </summary>
        /// <param name="parentXpath">父节点路径</param>
        /// <param name="childNodeName">子节点名称</param>
        public void RemoveChild(string parentXpath, string childNodeName)
        {
            if (string.IsNullOrWhiteSpace(parentXpath) || string.IsNullOrWhiteSpace(childNodeName))
                throw new ArgumentNullException();

            //获取父节点
            XmlNode parentNode = xmlDoc.SelectSingleNode(parentXpath);

            if (parentNode == null)
                throw new Exception("ParentNode is null");

            //获取子节点
            XmlNodeList nodeList = parentNode.ChildNodes;

            if (nodeList.Count > 0)
            {
                //循环子节点，删除第一个匹配的子节点
                foreach (var node in nodeList)
                {
                    var tmp = (XmlNode)node;

                    if (tmp.Name == childNodeName)
                    {
                        parentNode.RemoveChild(tmp);

                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 删除子节点
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="childNode">子节点</param>
        public void RemoveChild(XmlNode parentNode, XmlNode childNode)
        {
            if (parentNode == null || childNode == null)
                throw new ArgumentNullException();

            parentNode.RemoveChild(childNode);
        }

        #endregion

        #region 读取节点

        /// <summary>
        /// 获取指定路径下的节点
        /// 如果获取到，则返回得到的节点
        /// 如果获取不到，则返回null
        /// </summary>
        /// <param name="xpath"></param>
        /// <param name="index">可选参数，参数默认值0；如果该节点有多个，可以指定index，返回具体节点</param>
        /// <returns></returns>
        public XmlNode GetNode(string xpath, int index = 0)
        {
            XmlNodeList nodeList = hasNamespace ? xmlDoc.SelectNodes(xpath, nsManager) : xmlDoc.SelectNodes(xpath);

            if (nodeList != null && nodeList.Count >= index + 1)
                return nodeList[index];

            return null;
        }

        /// <summary>
        /// 返回指定路径下的节点列表
        /// 如果获取到，则返回得到的节点列表
        /// 如果获取不到，则返回空列表
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public XmlNodeList GetNodeList(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                throw new ArgumentNullException();

            return hasNamespace ? xmlDoc.SelectNodes(xpath, nsManager) : xmlDoc.SelectNodes(xpath);
        }

        /// <summary>
        /// 获取节点及其所有子节点的串联值
        /// 如果获取到，则返回节点串联值
        /// 如果获取不到，则返回空字符串
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public String GetInnerText(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                throw new ArgumentNullException();

            string rlt = string.Empty;

            XmlNode node =
                hasNamespace ? xmlDoc.SelectSingleNode(xpath, nsManager) : xmlDoc.SelectSingleNode(xpath);

            if (node != null)
                rlt = node.InnerText;

            return rlt;
        }

        /// <summary>
        /// 获取节点的值
        /// 返回的值取决于节点的 System.Xml.XmlNode.NodeType 类型。
        /// CDATASection类型，返回CDATA 节的内容。
        /// Comment类型，返回注释的内容。
        /// Text类型，返回文本节点的内容。
        /// ProcessingInstruction类型，返回全部内容（不包括指令目标）。 
        /// SignificantWhitespace类型，返回空白字符。空白可由一个或多个空格字符、回车符、换行符或制表符组成。
        /// Whitespace类型，返回空白字符。空白可由一个或多个空格字符、回车符、换行符或制表符组成。
        /// XmlDeclaration类型，返回声明的内容。
        /// </summary>
        /// <param name="xpath"></param>
        /// <returns></returns>
        public String GetValue(string xpath)
        {
            if (string.IsNullOrWhiteSpace(xpath))
                throw new ArgumentNullException();

            string rlt = string.Empty;

            XmlNode node =
                hasNamespace ? xmlDoc.SelectSingleNode(xpath, nsManager) : xmlDoc.SelectSingleNode(xpath);

            if (node != null)
                rlt = node.Value;

            return rlt;
        }

        /// <summary>
        /// 返回指定节点的特性值
        /// 如果获取到指定节点的特性，则返回属性值；
        /// 如果获取不到或者出错，则返回空字符串；
        /// </summary>
        /// <param name="xpath">特性所在节点路径。比如："/ns:sqlMapConfig/ns:database/ns:dataSource"</param>
        /// <param name="attributeName">特性名</param>
        /// <returns></returns>
        public String GetAttributeValue(string xpath, string attributeName)
        {
            if (string.IsNullOrWhiteSpace(xpath) || string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentNullException();

            string rlt = string.Empty;

            XmlNode node = hasNamespace ? xmlDoc.SelectSingleNode(xpath, nsManager) : xmlDoc.SelectSingleNode(xpath);

            if (node == null || node.Attributes == null)
                return rlt;

            //this[string name] 索引器，返回与指定名称匹配的第一个 XmlElement。如果匹配不到返回null(特性集合为null或者没有该特性)
            //用数字下标的索引器，集合为null会有异常
            XmlAttribute nodeAttribute = node.Attributes[attributeName];

            if (nodeAttribute != null)
                rlt = nodeAttribute.Value;

            return rlt;
        }

        /// <summary>
        /// 返回指定节点的特性值
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="attributeName">特性名称</param>
        /// <returns></returns>
        public String GetAttributeValue(XmlNode node, string attributeName)
        {
            if (node == null || string.IsNullOrWhiteSpace(attributeName))
                throw new ArgumentNullException();

            return node.Attributes != null ? node.Attributes[attributeName].Value : string.Empty;
        }

        /// <summary>
        /// 将Xml文档转换成DataSet
        /// </summary>
        /// <returns></returns>
        public DataSet GetDataSetByXml()
        {
            DataSet ds = new DataSet();

            ds.ReadXml(xmlFileName);

            return ds;
        }

        #endregion

        /// <summary>
        /// 获取xml文档
        /// </summary>
        /// <returns></returns>
        public XmlDocument GetDocument()
        {
            return xmlDoc;
        }

        #endregion
    }
}

