using System;
using System.IO;
using System.Xml.Serialization;
using WhatTheForkMVC.Classes.Restaurant;

namespace WhatTheForkMVC.Classes
{
    public class SearchSerializer
    {
        public Byte[] Serialize(SearchCriteria criteria)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SearchCriteria));
            MemoryStream memStream = new MemoryStream();
            Byte[] byteArray;

            serializer.Serialize(memStream, criteria);
            byteArray = memStream.ToArray();
            memStream.Close();

            return byteArray;
        }

        public SearchCriteria Deserialize(Byte[] byteArray)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(SearchCriteria));
            MemoryStream memStream = new MemoryStream(byteArray);

            SearchCriteria objCriteria = (SearchCriteria)serializer.Deserialize(memStream);
            memStream.Close();

            return objCriteria;
        }
    }
}
