using RandomNameGeneratorLibrary;
using System;
using System.Collections.Generic;
using DataGenerator.Models;

namespace DataGenerator
{
    public static class TweetGenerator
    {
        private static PersonNameGenerator _personNameGenerator;

        public static void Init()
        {
            _personNameGenerator = new PersonNameGenerator();
        }

        public static Tweet Generate()
        {
            var textWithHashtags = GetRandomTweetText();

            return new Tweet
            {
                CreatedAt = DateTime.Now,
                Text = textWithHashtags.Text,
                User = GetUser(),
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

        private static User GetUser()
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
            var contoso = "@ContosoAuto";

            var tweets = new List<string>
            {
                $"I love the new #{vehicle.Make} #{vehicle.Model} I purchased today from {contoso}!",
                $"{contoso} I'm extremely unhappy with the performance of my #{vehicle.Make} #{vehicle.Model}.",
                $"The best selling car in {location} is... #{vehicle.Make} #{vehicle.Model}. Buy one now at {contoso}!",
                $"The fuel efficiency of my #{vehicle.Model} is abysmal #{vehicle.Make}. Might be time to visit {contoso} for a new car.",
                $"The {contoso} press release about sales figures for the new #{vehicle.Make} #{vehicle.Model} has gotten the attention of investors.",
                $"Taking a road trip to {location} in my #{vehicle.Make} #{vehicle.Model}",
                $"Tailgating at the Eagles game in my #{vehicle.Make} #{vehicle.Model}. Thanks for the free tickets {contoso}!",
                $"Just purchased the new #{vehicle.Make} #{vehicle.Model} from {contoso}. It is a piece of art!",
                $"Vehicle shopping today at {contoso}. I'm not crazy about the design of the new #{vehicle.Make} #{vehicle.Model}.",
                $"Just hit 100,000 miles in my #{vehicle.Make} #{vehicle.Model}",
                $"Back at {contoso} to get my #{vehicle.Make} #{vehicle.Model} repaired again. Piece of junk!",
                $"{contoso}, when will the {DateTime.Now.AddYears(1)} #{vehicle.Make} #{vehicle.Model} be available?",
                $"Thank you for the amazing service at the {contoso} {location} service center #{vehicle.Make} #{vehicle.Model}",
                $"Watch out world! Hitting the road in my new #{vehicle.Make} #{vehicle.Model}",
                $"Got caught in a freak snow storm in {location}! My #{vehicle.Make} #{vehicle.Model} was sliding all over the road. Need to visit {contoso} for some new tires.",
                $"Celebrating 5 years of working at the {contoso} {location} facility making the #{vehicle.Make} #{vehicle.Model}",
                $"Enjoying the driving experience in my #{vehicle.Model} #{vehicle.Make}.",
                $"{location} bound in my #{vehicle.Make} #{vehicle.Model}.",
                $"The battery in my #{vehicle.Make} #{vehicle.Model} does not like the cold! {contoso}, any solutions to fix this?",
                $"The new #{vehicle.Make} #{vehicle.Model} is pretty!",
                $"Loving the design of the new #{vehicle.Make} #{vehicle.Model}.",
                $"Just hit 200,000 miles in my #{vehicle.Make} #{vehicle.Model}!",
                $"My 2015 #{vehicle.Make} #{vehicle.Model} is at {contoso} for repairs, again!",
                $"Lackluster service at the {contoso} {location} service center on my #{vehicle.Make} #{vehicle.Model}.",
                $"Heading to the {location} auto show in my #{vehicle.Make} #{vehicle.Model} to see the new {contoso} models.",
                $"{contoso}, can you make a #{vehicle.Make} #{vehicle.Model} that can go 300 mph please?",
                $"So sad, my #{vehicle.Make} #{vehicle.Model} was totaled today.",
                $"My #{vehicle.Make} #{vehicle.Model} is comfortable enough to sleep in.",
                $"#{vehicle.Make} #{vehicle.Model} for sale at the {contoso} lot in {location}. Six years old now, but it is in good shape and has low mileage.",
                $"Looking for a replacement vehicle after crashing my #{vehicle.Make} #{vehicle.Model}.",
                $"Really felt the cross wind driving my #{vehicle.Make} #{vehicle.Model} on the highway through {location} today.",
                $"Don't smoke in your #{vehicle.Make} #{vehicle.Model} if you want me to buy it!",
                $"Auto-braking in the #{vehicle.Make} #{vehicle.Model} saved me from an accident today.",
                $"Rented a #{vehicle.Make} #{vehicle.Model} from {contoso} for my trip. Great car!",
                $"Rumor's out of {contoso} suggest the #{vehicle.Make} #{vehicle.Model} could be replaced next years.",
                $"It costs a small fortune to put fuel in my #{vehicle.Make} #{vehicle.Model}.",
                $"I was thinking about my #{vehicle.Make} #{vehicle.Model} today.",
                $"In my #{vehicle.Make} #{vehicle.Model} headed for the {location} airport.",
                $"Hey person in the grey #{vehicle.Make} #{vehicle.Model}, learn to drive!",
                $"Selling everything, my #{vehicle.Make} #{vehicle.Model}, refrigerator, lawnmower, sprinkler system, smoke alarms, and even my clothes.",
                $"The rear end of my #{vehicle.Make} #{vehicle.Model} danced sideways, bouncing like a horse kicking up its heels, while driving offroad.",
                $"My #{vehicle.Make} #{vehicle.Model} has an amazing sound system installed by {contoso}.",
                $"My #{vehicle.Make} #{vehicle.Model} may not look like much, but it is reliable.",
                $"Saw the #{vehicle.Make} #{vehicle.Model} of tomorrow at the {contoso} car show. Beautiful!",
                $"The essence of my #{vehicle.Make} #{vehicle.Model} is that it takes me places I want to go.",
                $"Took the ferry across the sound to get my #{vehicle.Make} #{vehicle.Model} over to the park.",
                // Spanish tweets
                $"Mi {DateTime.Now.Year} #{vehicle.Make} #{vehicle.Model} es el mejor auto de todos!",
                $"¡El sistema de sonido en mi nuevo #{vehicle.Make} #{vehicle.Model} es increíble!",
                // French tweets
                $"La circulation à la frontière canadienne était terrible aujourd'hui. Heureusement, mon nouveau #{vehicle.Make} #{vehicle.Model} est confortable.",
                $"Quand le {DateTime.Now.AddYears(1)} #{vehicle.Make} #{vehicle.Model} sera-t-il disponible au Canada?"
            };

            var r = new Random();
            var c = tweets.Count;
            var num = r.Next(c);
            var tweetText = tweets[num];

            return new TweetTextWithHashtags
            {
                Text = tweetText,
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
                new Vehicle { Make = "Audi", Model = "A6"},
                new Vehicle { Make = "Audi", Model = "A8"},
                new Vehicle { Make = "Audi", Model = "R8"},
                new Vehicle { Make = "Chevrolet", Model = "Suburban"},
                new Vehicle { Make = "Chevrolet", Model = "Corvette"},
                new Vehicle { Make = "Chevrolet", Model = "Tahoe"},
                new Vehicle { Make = "Chevrolet", Model = "Malibu"},
                new Vehicle { Make = "Chevrolet", Model = "Colorado"},
                new Vehicle { Make = "Chevrolet", Model = "Silverado"},
                new Vehicle { Make = "Dodge", Model = "Caravan"},
                new Vehicle { Make = "Dodge", Model = "Dakota"},
                new Vehicle { Make = "Dodge", Model = "Durango"},
                new Vehicle { Make = "Dodge", Model = "Ram"},
                new Vehicle { Make = "Dodge", Model = "Charger"},
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
                "Richmond, VA",
                "Virginia Beach, VA",
                "Seattle, WA",
                "Madison, WI",
                "Jackson, WY" };

            var count = list.Count;
            var r = new Random();
            var num = r.Next(count);
            return list[num];
        }
    }
}