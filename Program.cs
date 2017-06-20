using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;

namespace ReadFile
{
    class Program
    {
        static void Main(string[] args)
        {
            int counter = 0;
            string line;
            string path = @"C:\Users\Rahul-PC\Downloads\Twilio\";
            string file1 = "twilio.json", file2 = "db.txt";


            string json = File.ReadAllText(path + file1);
            List<string> listTwilio = new List<string>();
            List<string> listDB = new List<string>();

            /*
            // Make a GET Request from Postman or Fiddler to https://{TwilioSSID}:{TwilioToken}@api.twilio.com/2010-04-01/Accounts/{TwilioSSID}/IncomingPhoneNumbers.json?PageSize=50000&Page=0
            //Store the json in twilio.json
            //

            Query your db and store column data in db.txt with numbers in format of AAABBBCCCC
           

            //
            */

            //Read Twilio Numbers
            JsonTextReader reader = new JsonTextReader(new StringReader(json));
            bool print = false;
            while (reader.Read())
            {
                if (print)
                {
                    //Console.WriteLine(reader.Value.ToString().Substring(1,11));
                    listTwilio.Add(reader.Value.ToString().Substring(2,10));
                }
                    
                if (reader.Value != null && reader.Value.ToString() == "phone_number")
                {

                    print = true;

                }
                else
                {
                    print = false;
                }
            }

            // Read Realeflow Numbers
            System.IO.StreamReader file =
               new System.IO.StreamReader(path + file2);
            while ((line = file.ReadLine()) != null)
            {
                listDB.Add(line.Trim());
                counter++;
            }

            file.Close();

            var RF_intersect_Twilio = listTwilio.Intersect(listDB).ToList();

            var twilioExcept = listTwilio.Except(listDB).ToList();

            string numbers = string.Join(",",twilioExcept);
            var dbExcept = listDB.Except(listTwilio).ToList();
            Console.WriteLine("Following Numbers are not in database but they are present on Twilio :\n" + dbExcept);


            var twExcept = listTwilio.Except(listDB).ToList();
            Console.WriteLine("Following Numbers are not in Twilio but they are present in your database :\n" + twExcept);
            Console.ReadLine();
        }
    }
}
