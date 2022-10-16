using System;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Numerics;
using System.IO;

namespace MyRSA
{
    public class RSA
    {
        public bool Selected { get; set; }
        public long PublicKey { get => n; private set { } }
        public string Name { get; private set; }

        private int[] primes = { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97, 101, 103, 107, 109, 113, 127, 131, 137, 139, 149, 151, 157, 163, 167, 173, 179, 181, 191, 193, 197, 199, 211, 223, 227, 229, 233, 239, 241, 251, 257, 263, 269, 271, 277, 281, 283, 293, 307, 311, 313, 317, 331, 337, 347, 349, 353, 359, 367, 373, 379, 383, 389, 397, 401, 409, 419, 421, 431, 433, 439, 443, 449, 457, 461, 463, 467, 479, 487, 491, 499, 503, 509, 521, 523, 541, 547, 557, 563, 569, 571, 577, 587, 593, 599, 601, 607, 613, 617, 619, 631, 641, 643, 647, 653, 659, 661, 673, 677, 683, 691, 701, 709, 719, 727, 733, 739, 743, 751, 757, 761, 769, 773, 787, 797, 809, 811, 821, 823, 827, 829, 839, 853, 857, 859, 863, 877, 881, 883, 887, 907, 911, 919, 929, 937, 941, 947, 953, 967, 971, 977, 983, 991, 997 };
        Random rand = new Random();
        //private int[] primes = { 1009, 1013, 1019, 1021, 1031, 1033, 1039, 1049, 1051, 1061, 1063, 1069, 1087, 1091, 1093, 1097, 1103, 1109, 1117, 1123, 1129, 1151, 1153, 1163, 1171, 1181, 1187, 1193, 1201, 1213, 1217, 1223, 1229, 1231, 1237, 1249, 1259, 1277, 1279, 1283, 1289, 1291, 1297, 1301, 1303, 1307, 1319, 1321, 1327, 1361, 1367, 1373, 1381, 1399, 1409, 1423, 1427, 1429, 1433, 1439, 1447, 1451, 1453, 1459, 1471, 1481, 1483, 1487, 1489, 1493, 1499, 1511, 1523, 1531, 1543, 1549, 1553, 1559, 1567, 1571, 1579, 1583, 1597, 1601, 1607, 1609, 1613, 1619, 1621, 1627, 1637, 1657, 1663, 1667, 1669, 1693, 1697, 1699, 1709, 1721, 1723, 1733, 1741, 1747, 1753, 1759, 1777, 1783, 1787, 1789, 1801, 1811, 1823, 1831, 1847, 1861, 1867, 1871, 1873, 1877, 1879, 1889, 1901, 1907, 1913, 1931, 1933, 1949, 1951, 1973, 1979, 1987, 1993, 1997, 1999 };
        long p, q, n, f, d;
        const long e = 17;
        public RSA(string name=null)
        {
            Name = name;
            int indexp = rand.Next(5, primes.Length-3);
            p = primes[indexp];
            q = primes[indexp+2];
            n = p * q;
            f = (p - 1) * (q - 1);//Функция Эйлера
            d = GCDEx(e, f); //находим обратное
            if (d < 0)
            {
                d = f + d;
            }
            //long test = ComputPrivateKey(e, f); //тоже работает
        }
        public RSA(int publickey,string name = null)
        {
            Name = name;
            n = publickey;
        }
        public string Encryption(string str)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = str.ToCharArray();
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //Encoding win1251 = Encoding.GetEncoding("Windows-1251");
            byte[] utf8bytes = Encoding.Unicode.GetBytes(chars);
            foreach(var code in utf8bytes)
            {
                //long temp = (long)(Math.Pow(code, e) % n);
                BigInteger bigInteger = BigInteger.ModPow(code, e, n);
              //  Console.WriteLine(bigInteger);
                //Console.WriteLine(Convert.ToString((int)temp, 16));
                result.Append(bigInteger.ToString("x"));
                //result.Append(Convert.ToChar((long)bigInteger));
                result.Append('&');
                //result.Append('*');
                //result.Append("\n"); по приколу для тестинга
                //Console.WriteLine(result.ToString());
            }
            
            //for (int i=0; i< utf8bytes.Length; i+=2)
            //{
            //    for(int j=i; j< 2; j++)
            //    {

            //    }
            //}
            return result.ToString();
        }
        public string Encryption2(string str)
        {
            StringBuilder result = new StringBuilder();
            char[] chars = str.ToCharArray();
            int [] codes = new int[chars.Length];
            for(int i=0; i<chars.Length; i++)
            {
                codes[i] = (int)chars[i];
            }
            foreach (var code in codes)
            {
                    BigInteger bigInteger = BigInteger.ModPow(code, e, n);
                    result.Append(bigInteger.ToString("x"));
                    result.Append('&');
            }
            return result.ToString();
        }
        public string Decryption(string str) // работает
        {
            StringBuilder result = new StringBuilder();
            byte[] temp = new byte[str.Length/16]; //массив байтов уже расшифрованых
            int index = 0; 
            for (int i=0; i<str.Length; i += 16)
            {
                string test = str.Substring(i, 16); // получаем первые 16 бит
                long value = Int64.Parse(test, System.Globalization.NumberStyles.HexNumber); // получаем десятичное представление
                BigInteger bigInteger = BigInteger.ModPow(value, d, n); // считаем Unicode код
                temp[index] = (byte)bigInteger;
                index++;
            }
            index = 0;
            for(int i = 0; i < str.Length / 32; i++) //выводим символы по двум байтам
            {
                result.Append(BitConverter.ToChar(temp,index)); //все подробности в перегрузке метода
                index += 2;
            }
            return result.ToString();
        }
        public string Decryption2(string str) // работает
        {
            StringBuilder result = new StringBuilder();
            int[] temp = new int[str.Length]; //массив байтов уже расшифрованых
            int index = 0;
            int end = 0;
            string code = "";
            for (int i = 0; i < str.Length;)
            {
                for(int j = index; j < str.Length; j++)
                {
                    if (str[j] == '&') break;
                    code += str[j];
                    index++;
                }
                index++;
                i = index;
                try
                {
                    long value = (Int64.Parse(code, System.Globalization.NumberStyles.HexNumber));
                    BigInteger bigInteger = BigInteger.ModPow(value, d, n);
                    result.Append(Convert.ToChar((int)bigInteger));
                }
                catch
                {
                    return "Ошибка, возможно выбран не правильный ключ.";
                }
                //temp[end] = (byte)bigInteger;
                //result.Append(Convert.ToChar((int)bigInteger));
                //result.Append(BitConverter.ToChar(temp, end));
                code = "";
                end++;
            }
            index = 0;
            
            return result.ToString();
        }
        long ComputPrivateKey(long e, long module)
        {
            var d = 1;

            while ((d * e - 1) % module != 0) d++;

            return d;
        }

        private long GCD(long a, long b) //Gratest Common divisior
        {
            while (a != 0)
            {
                var temp = a;
                a = b % a;
                b = temp;
            }
            return b;
        }
        private bool SoPrime(long a, long b)
        {
            if(GCD(a,b)==1) return true;
            return false;
        }
        private int GCDEx(long a, long b) // разобраться в этом алгоритме, сложно я тупой
        {
            long x0, xn = 1;
            long y0, yn = 0;
            long x1 = 0;
            long y1 = 1;
            long r = a % b;
            x0 = 1;
            y0 = 1;
            while (r > 0)
            {
                long temp = a / b;
                xn = x0 - temp * x1;
                yn = y0 - temp * y1;

                x0 = x1;
                y0 = y1;
                x1 = xn;
                y1 = yn;
                a = b;
                b = r;
                r = a % b;
            }
            //Console.WriteLine($"{xn}=xn \n {yn}=yn \n{b}=b"); // xn обратное по модулю
            return (int)xn;
        }
    }
}
