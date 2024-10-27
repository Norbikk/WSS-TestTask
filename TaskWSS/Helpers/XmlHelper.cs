using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace TaskWSS.Helpers;

public static class XmlHelper
{
    public static string SerializeToXml<T>(T obj)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        var xmlSerializer = new XmlSerializer(typeof(T));
        using (var stringWriter = new StringWriter())
        {
            using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
            {
                xmlSerializer.Serialize(xmlWriter, obj);
                return stringWriter.ToString();
            }
        }
    }

    public static async Task<T> DeserializeAsync<T>(Stream xmlStream)
    {
        if (xmlStream == null || xmlStream.Length == 0)
            throw new ArgumentNullException(nameof(xmlStream));

        var xmlSerializer = new XmlSerializer(typeof(T));
        using (var streamReader = new StreamReader(xmlStream, Encoding.UTF8))
        {
            var xml = await streamReader.ReadToEndAsync();
            using (var stringReader = new StringReader(xml))
            {
                var result = (T)xmlSerializer.Deserialize(stringReader);
                if (result == null)
                    throw new InvalidOperationException("Deserialization resulted in a null object.");
                return result;
            }
        }
    }
}