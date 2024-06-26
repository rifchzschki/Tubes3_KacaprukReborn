using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class ConvertAlay{
    private void parseAngka(ref string str){
        Dictionary<char, char> pasangan = new Dictionary<char, char>();
        // Menambahkan elemen ke dalam map
        pasangan['4'] = 'a';
        pasangan['3'] = 'e';
        pasangan['6'] = 'g';
        pasangan['1'] = 'i';
        pasangan['0'] = 'o';
        pasangan['5'] = 's';
        pasangan['2'] = 'z';

        // Mengganti karakter berdasarkan map
        str = Regex.Replace(str, @"[0-6]", m =>
        {
            char c = m.Value[0];
            if (pasangan.ContainsKey(c))
            {
                return pasangan[c].ToString();
            }
            else
            {
                return c.ToString();
            }
        });
    }

    private bool validasi(string alay, string? ori)
    {
        if(ori == null){
            return false;
        }
        
        // ubah awal dan acuan ke bentuk lower case
        // ubah angka yang terdapat pada alay menjadi huruf

        alay = alay.ToLower();
        ori = ori.ToLower();


        parseAngka(ref alay);

        // validasi setiap karakter, dengan constraint -> jika ori[i] adalah vokal maka maklumi jika di alay[j]!=ori[i] (asumsi alay[j] adalah konsonan). geser ori hingga menemukan konsonan. 
        // jika karakter sama maka next
        // jika karakter beda maka cek dulu
        // jika ori[i] adalah vokal dan alay[i] adalah konsonan cek lagi
        // simpan index konsonan ke j
        // geser i hingga ori menemukan konsonan
        // cek apakah ori[i] = konsonan[j], jika beda return false, jika sama lanjut ke ori berikutnya dan dari awal, tapi gimana caranya index konsonan lanjut dari j

        //hello word
        //hll word

        int i = 0;
        int j = 0;
        while (j < alay.Length)
        {
            // Console.WriteLine(ori[i]);
            // Console.WriteLine(i);
            if (ori[i] == alay[j])
            {
                i++;
                j++;
                continue;
            }
            else
            {
                if (Regex.IsMatch(ori[i].ToString(), @"^[aeiouAEIOU]$") && !Regex.IsMatch(alay[j].ToString(), @"^[aeiouAEIOU]$"))
                {
                    while (Regex.IsMatch(ori[i].ToString(), @"^[aeiouAEIOU]$"))
                    {
                        i++;
                    }
                    if (ori[i] != alay[j])
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            j++;
            i++;
            // Test.TestHere(ori);
        }
        return true;

    }

    public static string? findAlayMatch(List<string> alays, string? asli)
    {
        ConvertAlay program = new ConvertAlay();
        // string acuan = "buntng";
        // string input = "bunting";
        foreach (var alay in alays){
            if (program.validasi(alay, asli)){
                // Test.TestHere(alay);
                return alay;
            }
        }
        return null;

    }
}
