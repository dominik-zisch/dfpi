using System.Text;

public class MqttUtils
{
    public static string ByteArrayToHexString(byte[] ba)
    {
        StringBuilder hex = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba)
            hex.AppendFormat("{0:x2}:", b);
        var str = hex.ToString();
        return str.Remove(str.Length - 1, 1);
    }

    public static byte[] HexStringToByteArray(string str)
    {
        var tokens = str.Split(':');
        byte[] bytes = new byte[tokens.Length];
        for (int i = 0; i < tokens.Length; i++)
        {
            bytes[i] = byte.Parse(tokens[i], System.Globalization.NumberStyles.HexNumber);
        }
        return bytes;
    }
}
