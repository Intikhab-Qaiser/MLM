using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLM
{
    class Program
    {
        static void Main(string[] args)
        {
            int rows = 5; int cols = 6; 

            // Add houses
            List<House> houses = new List<House>();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    houses.Add(new House(new Location { XPos = i, YPos = j }));
                }
            }

            // Add 1 person to each house
            List<Person> persons = new List<Person>();
            foreach (var h in houses)
            {
                if (h.Location.XPos == 0 && h.Location.YPos == 0)
                    persons.Add(new Person(h, PersonType.Salesman, h.Location));
                else
                    persons.Add(new Person(h, PersonType.Unemployed, h.Location));
            }

            // Loop while we have any Unemployed person within any house
            while(true)
            {
                List<Person> salesMen = persons.Where(m => m.PersonType == PersonType.Salesman).ToList();
                List<Person> unEmployed = persons.Where(m => m.PersonType == PersonType.Unemployed).ToList();
                if (unEmployed.Count == 0)
                    return;

                foreach (var p in salesMen)
                {
                    int right = p.House.Location.XPos + 1;
                    int left = p.House.Location.XPos - 1;
                    int up = p.House.Location.YPos + 1;
                    int below = p.House.Location.YPos - 1;

                    if (right < cols)
                    {
                        p.CurrentLocation.XPos += 1;
                    }
                    else if (left > 0)
                    {
                        p.CurrentLocation.XPos -= 1;
                    }
                    else if (up < rows)
                    {
                        p.CurrentLocation.YPos += 1;
                    }
                    else if (below > 0)
                    {
                        p.CurrentLocation.YPos -= 1;
                    }

                    House house = houses.Where(h => h.Location == p.CurrentLocation).SingleOrDefault();
                    Person person = unEmployed.Where(pr => pr.House.Location.XPos == house.Location.XPos && 
                                                        pr.House.Location.YPos == house.Location.YPos).SingleOrDefault();
                    if (person != null)
                    {
                        person.PersonType = PersonType.Salesman;
                    }
                }
            }
        }
    }

    // Person type enum
    enum PersonType
    {
        Unemployed,
        Salesman
    }

    // House class with Location property
    class House
    {
        public House(Location location)
        {
            Location = location;
        }

        public Location Location { get; set; }
    }
    
    // Person class with properties to have PersonType, Current Location and to which House it belongs
    class Person
    {
        public Person(House house, PersonType personType, Location location)
        {
            PersonType = personType;
            House = house;
            CurrentLocation = location;
        }

        public PersonType PersonType { get; set; }

        public House House { get; set; }

        public Location CurrentLocation { get; set; }
    }

    // Location class for getting position for House and Person
    class Location
    {
        public int XPos { get; set; }

        public int YPos { get; set; }
    }
}
