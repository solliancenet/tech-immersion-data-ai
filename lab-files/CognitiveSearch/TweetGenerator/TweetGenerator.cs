using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using Twitter.Common.Models;

namespace TweetGenerator
{
    public static class TweetGenerator
    {
        //private static readonly Random Random = new Random();

        private static PersonNameGenerator _personNameGenerator = new PersonNameGenerator();

        public static void Init()
        {

        }

        public static Tweet GenerateTweet()
        {
            var textWithHashtags = GetRandomTweetText();

            return new Tweet
            {
                CreatedAt = DateTime.Now,
                Text = textWithHashtags.Text,
                User = GetUser(1),
                Entities = new Entity
                {
                    Hashtags = textWithHashtags.Hashtags,
                    UserMentions = new UserMention[]
                    {
                        new UserMention
                        {
                            Id = 2244994945,
                            Indices = new int[2] { 0, 12 },
                            Name = "Contoso Auto",
                            ScreenName = "ContosoAuto"
                        }
                    },
                    Symbols = new Symbol[0],
                    Urls = new Url[0]
                }
            };
        }

        private static User GetUser(int id)
        {
            return new User
            {
                Name = _personNameGenerator.GenerateRandomFirstAndLastName(),
            };
        }

        private static TweetTextWithHashtags GetRandomTweetText()
        {
            var vehicle = GetVehicle();
            var location = GetLocation();

            var tweets = new List<string>
            {
                $"I love my new #{vehicle.Make} #{vehicle.Model}",
                $"Not happy with the performance of my #{vehicle.Make} #{vehicle.Model}",
                $"Fuel efficiency of my #{vehicle.Model} is abysmal #{vehicle.Make}",
                $"Road trip to {location} in my #{vehicle.Make} #{vehicle.Model}",
                $"Tailgating at the Eagles game in my #{vehicle.Make} #{vehicle.Model}",
                $"The new #{vehicle.Make} #{vehicle.Model} is a piece of art",
                $"Not crazy about the design of the new #{vehicle.Make} #{vehicle.Model}",
                $"Just hit 100,000 miles in my #{vehicle.Make} #{vehicle.Model}",
                $"In the shop again with my #{vehicle.Make} #{vehicle.Model}. Piece of junk!",
                $"When will the {DateTime.Now.AddYears(1)} #{vehicle.Make} #{vehicle.Model} be available?",
                $"Thank you for the amazing service at the {location} service center #{vehicle.Make} #{vehicle.Model}",
                $"Watch out world! Hitting the road in my new #{vehicle.Make} #{vehicle.Model}",
                $"Freak snow storm in {location}! My #{vehicle.Make} #{vehicle.Model} was sliding all over the road.",
                $"Celebrating 5 years of working at the {location} facility making the #{vehicle.Make} #{vehicle.Model}",
                $"Enjoying the driving experience in my #{vehicle.Model} #{vehicle.Make}",
                $"{location} bound in my #{vehicle.Make} #{vehicle.Model}",
                $"The battery in my #{vehicle.Make} #{vehicle.Model} does not like the cold!",
                $"The new #{vehicle.Make} #{vehicle.Model} is pretty!",
                $"Loving the design of the new #{vehicle.Make} #{vehicle.Model}.",
                $"Just hit 200,000 miles in my #{vehicle.Make} #{vehicle.Model}",
                $"2015 #{vehicle.Make} #{vehicle.Model} is in for repairs, again!",
                $"Lackluster service at the {location} service center on my #{vehicle.Make} #{vehicle.Model}.",
                $"Heading to the {location} auto show in my #{vehicle.Make} #{vehicle.Model} to see the new ContosoAuto models.",
                $"Mi {DateTime.Now.Year} #{vehicle.Make} #{vehicle.Model} es el mejor auto de todos!",
                $"Could we make a #{vehicle.Make} #{vehicle.Model} that can go 300 mph?",
                $"So sad, my #{vehicle.Make} #{vehicle.Model} was totaled today.",
                $"My #{vehicle.Make} #{vehicle.Model} is comfortable enough to sleep in.",
                $"#{vehicle.Make} #{vehicle.Model} for sale. Six years old now, but it is in good shape and has low mileage.",
                $"Looking for a replacement vehicle after crashing my #{vehicle.Make} #{vehicle.Model}.",
                $"Really felt the cross wind driving my #{vehicle.Make} #{vehicle.Model} on the highway new {location} today.",
                $"Don't smoke in your #{vehicle.Make} #{vehicle.Model} if you want me to buy it!",
                $"Auto-braking in the #{vehicle.Make} #{vehicle.Model} saved me from an accident today.",
                $"Rented a #{vehicle.Make} #{vehicle.Model} for my trip.",
                $"Rumor has it that the #{vehicle.Make} #{vehicle.Model} could be replaced in the next few years.",
                $"Costs a small fortune to put gas in my #{vehicle.Make} #{vehicle.Model}.",
                $"I was thinking about my #{vehicle.Make} #{vehicle.Model} today.",
                $"In my #{vehicle.Make} #{vehicle.Model} headed for the {location} airport.",
                $"Person in the grey #{vehicle.Make} #{vehicle.Model}, learn to drive!",
                $"Selling everything, my #{vehicle.Make} #{vehicle.Model}, refrigerator, lawnmower, sprinkler system, smoke alarms, and even my clothes.",
                $"The rear end of my #{vehicle.Make} #{vehicle.Model} danced sideways, bouncing like a horse kicking up its heels, while driving offroad.",
                $"My #{vehicle.Make} #{vehicle.Model} has an amazing sound system.",
                $"My #{vehicle.Make} #{vehicle.Model} may not look like much, but it is reliable.",
                $"Saw the #{vehicle.Make} #{vehicle.Model} of tomorrow at the car show. Awesome!",
                $"The essence of my #{vehicle.Make} #{vehicle.Model} is that it takes me places I want to go.",
                $"Took the ferry across the sound to get my #{vehicle.Make} #{vehicle.Model} over to the park."
            };

            var r = new Random();
            var c = tweets.Count;
            var num = r.Next(c);
            var tweetText = tweets[num];

            var text = $"@ContosoAuto {tweetText}";

            return new TweetTextWithHashtags
            {
                Text = text,
                Hashtags = new Hashtag[]
                {
                    new Hashtag{ Text = vehicle.Make },
                    new Hashtag{ Text = vehicle.Model },
                }
            };
        }

        private static Vehicle GetVehicle()
        {
            var list = new List<Vehicle>
            {
                new Vehicle { Make = "Audi", Model = "A4"},
                new Vehicle { Make = "Chevrolet", Model = "Suburban"},
                new Vehicle { Make = "Chevrolet", Model = "Corvette"},
                new Vehicle { Make = "Chevrolet", Model = "Tahoe"},
                new Vehicle { Make = "Chevrolet", Model = "Malibu"},
                new Vehicle { Make = "Dodge", Model = "Caravan"},
                new Vehicle { Make = "Dodge", Model = "Dakota"},
                new Vehicle { Make = "Dodge", Model = "Durango"},
                new Vehicle { Make = "Dodge", Model = "Ram"},
                new Vehicle { Make = "Ford", Model = "Expedition"},
                new Vehicle { Make = "Ford", Model = "Explorer"},
                new Vehicle { Make = "Ford", Model = "F150"},
                new Vehicle { Make = "Ford", Model = "Mustang"},
                new Vehicle { Make = "Honda", Model = "Civic"},
                new Vehicle { Make = "Jeep", Model = "Wrangler"},
                new Vehicle { Make = "Nissan", Model = "Maxima"},
                new Vehicle { Make = "Subaru", Model = "Forester"},
                new Vehicle { Make = "Tesla", Model = "Sportster"},
                new Vehicle { Make = "Tesla", Model = "Model3"},
                new Vehicle { Make = "Toyota", Model = "Tacoma"},
                new Vehicle { Make = "Toyota", Model = "Tundra"},
                new Vehicle { Make = "Toyota", Model = "4Runner"},
                new Vehicle { Make = "Toyota", Model = "Carolla"},
                new Vehicle { Make = "Volkswagon", Model = "GTI"}
            };

            var r = new Random();
            var l = list.Count;
            var num = r.Next(l);
            return list[num];
        }

        private static string GetLocation()
        {
            var list = new List<string>
            {
                "Mobile, AL",
                "Phoenix, AZ",
                "Tucson, AZ",
                "San Diego, CA",
                "San Francisco, CA",
                "San Jose, CA",
                "Los Angeles, CA",
                "Sacramento, CA",
                "Redding, CA",
                "Anaheim, CA",
                "Long Beach, CA",
                "Fresno, CA",
                "Denver, CO",
                "Dover, DE",
                "Miami, FL",
                "Orlando, FL",
                "Tampa, FL",
                "Chicago, IL",
                "Indianapolis, IN",
                "Kansas City, KS",
                "Boston, MA",
                "Detroit, MI",
                "St. Louis, MO",
                "Las Vegas, NV",
                "New York, NY",
                "Portland, OR",
                "Philadelphio, PA",
                "Dallas, TX",
                "Austin, TX",
                "Houston, TX",
                "Virginia Beach, VA",
                "Seattle, WA",
                "Madison, WI",
                "Jackon, WY" };

            var count = list.Count;
            var r = new Random();
            var num = r.Next(count);
            return list[num];
        }
    }
}