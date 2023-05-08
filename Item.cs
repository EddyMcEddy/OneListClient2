using System;

namespace OneListClient2
{
    public class Item
    {


        public int id { get; set; }
        public string text { get; set; }
        public bool complete { get; set; }
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }

        //Creating our own GET so if property complete is TRUE then it will say completed.
        public string CompletedStatus
        {

            get
            {
                if (complete == true)
                {
                    return "Completed";
                }
                else
                {
                    return "Not Completed";
                }

            }

        }

    }
}