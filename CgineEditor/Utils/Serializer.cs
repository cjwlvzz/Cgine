using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace CgineEditor.Utils
{
    public static class Serializer
    {
        public static void ToFile<T>(T instance, string path)
        {
            try
            {
                using var fs = new FileStream(path, FileMode.Create);
                var serializer = new DataContractSerializer(typeof(T));
                serializer.WriteObject(fs,instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                // TODO: log error in engine editor
            }
        }
    }
}
