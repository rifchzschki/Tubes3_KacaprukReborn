using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

/// <summary>The BM class contains the Boyer-Moore string search algorithm.</summary>
class BM {
    /// <summary>The value of NO_OF_CHARS is 256.</summary>
    static int NO_OF_CHARS = 256;
 
    /// <summary>The max method returns the maximum of two integers.</summary>
    /// <returns>The greater of the two integers.</returns>
    static int Max(int a, int b) { return (a > b) ? a : b; }
 
    /// <summary> The badCharHeuristic method preprocesses the pattern string and creates a bad character array.</summary>
    /// <returns>badchar array</returns>
    static void BadCharHeuristic(string str, int size, int[] badchar)
    {
        int i;
 
        // Initialize all occurrences as -1
        for (i = 0; i < NO_OF_CHARS; i++) {
            badchar[i] = -1;
        }
 
        // Fill the actual value of last occurrence of a character
        for (i = 0; i < size; i++) {
            badchar[(int)str[i]] = i;
        }
    }
 
    // Fungsi utama untuk pencarian string
    /// <summary>The Search method takes a series of parameters to specify the search criterion and returns a dataset containing the result set. </summary>
    /// <returns>A DataSet instance containing the matching rows. It contains a maximum number of rows specified by the maxRows parameter</returns>
    static bool BMSearch(string text, string pattern)
    {
        int m = pattern.Length;
        int n = text.Length;
        
        int[] badchar = new int[NO_OF_CHARS];
 
        /* Fill the bad character array by calling badCharHeuristic() */
        BadCharHeuristic(pattern, m, badchar);
 
        int s = 0; // s adalah pergeseran pola terhadap teks
        while (s <= (n - m)) {
            int j = m - 1;
 
            // ngurangin index j selama karakter pola dan teks cocok
            while (j >= 0 && pattern[j] == text[s + j])
                j--;
 
            // jika j = -1, maka pola ditemukan
            if (j < 0) {
                // Console.WriteLine("Patterns occur at shift = " + s);
 
                // Geser pola agar karakter selanjutnya di teks sejajar dengan kemunculan terakhirnya di pola
                // Kondisi s+m < n diperlukan untuk kasus ketika pola terjadi di akhir teks
                s += (s + m < n) ? m - badchar[text[s + m]] : 1;
                return true;
            }
 
            else
                // Geser pola agar badchar di teks sejajar dengan kemunculan terakhirnya di pola
                // Fungsi Max digunakan untuk memastikan bahwa kita mendapatkan pergeseran positif
                // Kita mungkin mendapatkan pergeseran negatif jika kemunculan terakhir karakter buruk di pola berada di sebelah kanan current char
                s += Max(1, j - badchar[text[s + j]]);
        }

        return false;
    }

    /// <summary>
    /// Fungsi driver utama untuk multithreading, map isinya adalah dictionary yang berisi ascii sama tuple (path, nama orangnya)
    /// </summary>
    /// <returns>string: nama asli orang</returns>
    public static string? FindPatternInTexts(string pattern, Dictionary<string, (string, string)> map)
    {
        var cancellationTokenSource = new CancellationTokenSource();
        var tasks = new List<Task<string?>>();

        foreach (var text in map.Keys)
        {
            tasks.Add(Task.Run(() =>
            {
                if (BMSearch(text, pattern))
                {
                    cancellationTokenSource.Cancel();
                    return text;
                }
                return null;
            }, cancellationTokenSource.Token));
        }

        try
        {
            Task.WaitAny([.. tasks], cancellationTokenSource.Token);
        }
        catch (OperationCanceledException)
        {
        }

        foreach (var task in tasks)
        {
            if (task.IsCompletedSuccessfully && task.Result != null)
            {
                return map[task.Result].Item2;
            }
        }

        return null;
    }
 
    // Driver program to testing 
    // public static void Main()
    // {
    //     string txt = "ABAAABCD";
    //     string pat = "ABC";
    //     // Make dummy dictionary
    //     Dictionary<string, (string, string)> map = new Dictionary<string, (string, string)>();
    //     map.Add(txt, ("path", "berhasil"));
    //     map.Add("ABzCD", ("path", "name"));
    //     map.Add("ABCcD", ("path", "name"));
    //     map.Add("ABCDas", ("path", "name"));
    //     map.Add("ABCcsD", ("path", "name"));
    //     map.Add("ABCcdsD", ("path", "name"));
    //     map.Add("ABCDcsd", ("path", "name"));
    //     map.Add("ABCcdD", ("path", "name"));

    //     // string result = FindPatternInTexts(pat, map);
    //     // Console.WriteLine(FindPatternInTexts(pat, map));

    //     if (FindPatternInTexts(pat, map) != null) {
    //         Console.WriteLine("Pattern found");
    //         Console.WriteLine(FindPatternInTexts(pat, map));
    //     }
    //     else {
    //         Console.WriteLine("Pattern not found");
    //     }
    // }
}