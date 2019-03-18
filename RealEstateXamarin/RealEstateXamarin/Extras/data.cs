using RealEstateXamarin.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateXamarin.Extras
{
    class Data
    {
        private static int[] minPrices = new int[] {0,0,500000,750000,1000000,1500000,2000000};
        private static int[] maxPrices = new int[] {0,500000, 750000, 1000000, 1500000, 2000000, 0 };
        private static string[] priceRanges = new
        string[] {  "Any",
                    "-50 millon",
                    "50 million-75 million",
                    "75 million-100 million",
                    "100 million-150 millons",
                    "150 millon - 200 million",
                    "200 millon +" };
        private static string[] prefectures = new
        string[] { "Aichi", "Akita", "Aomori", "Chiba", "Ehime", "Fukui", "Fukuoka", "Fukushima",
            "Gifu", "Gumma", "Hiroshima", "Hokkaido", "Hyogo", "Ibaraki", "Ishikawa", "Iwate", "Kagawa",
            "Kagoshima", "Kanagawa", "Kochi", "Kumamoto", "Kyoto", "Mie", "Miyagi", "Miyazaki", "Nagano",
            "Nagasaki", "Nara", "Niigata", "Oita", "Okayama", "Okinawa", "Osaka", "Saga", "Saitama", "Shiga",
            "Shimane", "Shizuoka", "Tochigi", "Tokushima", "Tokyo", "Tottori", "Toyama", "Wakayama", "Yamagata",
            "Yamaguchi", "Yamanashi" };

        private string[] TokyoDistricts = new
        string[] { "Chiyoda","Chūō","Minato","Shinjuku","Bunkyō","Taitō","Sumida"
        ,"Kōtō","Shinagawa","Meguro","Ōta","Setagaya","Shibuya"
        ,"Nakano","Suginami","Toshima","Kita","	Arakawa","Itabashi"
        ,"Nerima","Adachi","Katsushika","Edogawa"};


        public static Search setPrices(bool Yen,Search search,string range) {
            int adjuster = 0;
            if (Yen) {
                adjuster = 100;
            }

            for (int i = 0; i < priceRanges.Length; i++) {
                if (range.Equals(priceRanges[i])) {
                    search.PriceMin = minPrices[i] * adjuster;
                    search.PriceMax = maxPrices[i] * adjuster;
                }
            }

            return search;
        }
        
        public static List<String> getPrefectures() {
            List<string> puerca = new List<string>();
            foreach (string shua in prefectures) {
                puerca.Add(shua);
            }
            return puerca;
        }
        public static List<String> getPrices()
        {
            List<string> puerca = new List<string>();
            foreach (string shua in priceRanges)
            {
                puerca.Add(shua);
            }
            return puerca;
        }

    }
}
