﻿using System.Collections.Generic;

namespace LIE
{
    public static class KnowledgeBase
    {
        public static class Excludes
        {
            public static string[] FeaturedMarkers = new[] {"(", ")"};

            public static string PlaceholderAlbumArtists = "Various Artists";

            public static char[] CharactersSeparatorsForWords = new[] {',','/',';',':','&','+'};

            //public static string[] RemainingPhrasesGlobal = new[]
            //{
            //};

            //public static string[] RemainingPhrasesFromFeatures= new string[]
            //{
            //    "dub",

            //};

            //public static char[] WordsSeparatorsForFeatures = new[] {' ', '\'', '"'};

            public static string[] WordsSeparatorsGlobal = new[]
            {
                "and",
                "arr.",
                "by",
                "ch",
                "con",
                "demo",
                "edit",
                "extended",
                "feat",
                "featuring",
                "feat.",
                "ft.",
                "instrumental",
                "main",
                "mix",
                "mixed",
                "original",
                "part",
                "presents",
                "pres.",
                "prod.",
                "radio",
                "remix",
                "remixed",
                "theme",
                "version",
                "vocal",
                "vocals",
                "vs",
                "vs.",
                "with",
                "x",
                "✖",
            };

            public static string[] ArtistsForSplitting = new[]
            {
                "2pac",
                "alvin pleasant delaney carter",
                "anna of the north",
                "antonio carles marques pinto",
                "aram mp3",
                "armistead burwell smith iv",
                "Arthur Herzog, Jr.",
                "arvo part",
                "Aywood Magic Johnson, Jr.",
                "Berry Gordy, Jr.",
                "b l a c k i e",
                "boydston john d. iii",
                "William Calhoun, Jr.",
                "Chris Powers, Jr",
                "Clint Ballard, Jr.",
                "D'Angelo Lacy",
                "Damian Kulash, Jr.",
                "Dolores O'Riordan",
                "Emilio Estefan, Jr.",
                "Felix Stallings, Jr.",
                "George Anderson, Jr.",
                "George Clinton, Jr.",
                "George Nash, Jr.",
                "Harry Middlebrooks, Jr.",
                "jennifer evans van der harten",
                "john 5",
                "K-$",
                "kh of moscrill",
                "Larry Mullen, Jr.",
                @"luther ""guitar junior"" johnson",
                "maria isabel garcia asensio",
                "Martin Luther King, Jr.",
                "Mary Margaret O'Hara",
                "michael l. williams ii",
                "michiel van der kuy",
                "niels henning orsted pedersen",
                "Oscar Brown, Jr.",
                "Otis Jackson, Jr.",
                "Possessed by Paul James",
                "the reverend peyton",
                "Ta'Raach",
                "Sammy James, Jr.",
                "William E. Cobham, Jr.",

            };

            public static string[] BandsForSplitting = new[]
            {
                
                "2000 Years BC",
                "4 Hero",
                "50 & 50 Brothers",
                "Above & Beyond",
                "AC/DC",
                "AC; DC",
                "Adam and the Ants",
                "Agnelli & Nelson",
                "A Hawk And A Hacksaw",
                "Al Jones & His Band",
                "Angus & Julia Stone",
                "Ant & Dec",
                "Antony and the Johnsons",
                "Arms and Legs",
                "Arms and Sleepers",
                "Art Blakey & The Jazz Messengers",
                "Artist Vs Poet",
                "Astronauts, etc.",
                "Awon & Phoniks",
                "Balaam And the Angel",
                "B. Bumble & The Stingers",
                "Beans & Company",
                "Beasts and Superbeasts",
                "Belle And Sebastian",
                "Benny Sharp & His Orchestra",
                "Benny Turner and Real Blues",
                "Between the Buried and Me",
                "Bigga & Bolda",
                "Bill and Belle Reed",
                "Bill Haley & His Comets",
                "Billy King & the Bad Bad Bad",
                "Bird by Bird",
                "Bizkit with Butta On the Keytar",
                "The Black & White Years",
                "Blood And Roses",
                "Blu And Exile",
                "Bob Reed & His Band",
                "Bob Zuga & His Orchestra",
                "Body & Soul",
                "Bogart and the Addictives",
                "Bonnie Whitmore & Her Band",
                "Boy & Bear",
                "Boy Meets Girl",
                "Black & Brown",
                "Blood, Sweat & Tears",
                "Blue Rodeo & Friends",
                "Boy & Bear",
                "Bread Love And Dreams",
                "The Break And Repair Method",
                "Brighton, MA",
                "Buddy & Babe",
                "Buddy Guy & His Band",
                "The Builders and The Butchers",
                "Built By Snow",
                "Busty and the Bass",
                "Can 7",
                "Captain Beefheart & His Magic Band",
                "Catfish And The Bottlemen",
                "Catfish & the Dogstars",
                "Cheeky Cheeky And The Nosebleeds",
                "Chimes & Bells",
                "Christine And The Queens",
                "C.J. & Co",
                "CK West & Co",
                "Cliff Driver & His Drivers",
                "Clyde Doerr and his Orchestra",
                "Con Brio",
                "Conductor And The Cowboy",
                "Conga Radio",
                "Construction & Destruction",
                "Copilot Music + Sound",
                "Count Basie & His Orchestra",
                "Creature With The Atom Brain",
                "Crosby, Stills, Nash & Young",
                "Croy and the Boys",
                "Damian Cowell's Disco Machine",
                "Dance With the Dead",
                "Daniel Ellsworth & The Great Lakes",
                "Daphne Willis and Co.",
                "Dave Dee Dozy Beaky Mick & Tich",
                "Dave And Ansell Collins",
                "Dawn and Hawkes",
                "Dear and the Headlights",
                "Death By Unga Bunga",
                "Deidre & the Dark",
                "Delaney & Bonnie & Friends",
                "Dent May & His Magnificent Ukulele",
                "Der Original Schintl",
                "Die original Ochsenfurter Blasmusik",
                "Dionne & Friends",
                "Disco Dream And The Androids",
                "Divide And Dissolve",
                "Doctor And The Medics",
                "The Dove & the Wolf",
                "Dr. Bobby Banner, MPC",
                "Drew Smith and His Band",
                "Drive-By Truckers",
                "Driving By Night",
                "Drop Dead, Gorgeous",
                "Drums & Tuba",
                "Duke Robillard and Roomfull Of Blues",
                "The Dutchess & the Duke",
                "Earth, Wind & Fire",
                "Echo & The Bunnymen",
                "Elana James and Her Hot, Hot Trio",
                "Emerson, Lake & Palmer",
                "Esther and Abi Ofarim",
                "Ex:Re",
                "Facts & Fiction",
                "Family and Friends",
                "Fighting With Wire",
                "Flash & The Pan",
                "Florence + The Machine",
                "Followed By Static",
                "The Frank And Walters",
                "Frankie and the Witch Fingers",
                "Friends Lovers & Family",
                "Fripps & Fripps",
                "Frnkiero and the Cellabration",
                "Fruit & Flowers",
                "Future Clouds And Radar",
                "F & W",
                "Gina X Performance",
                "Girls Guns and Glory",
                "Glam Sam and his Combo",
                "Gods and Monsters",
                "Good Night & Good Morning",
                "The Good, The Bad and The Queen",
                "Goombay Dance Band",
                "Gregory Pepper and His Problems",
                "Gus Cannon & His Jug Stompers",
                "Gus Jinkins & Orchestra",
                "Hans Gruber and the Die Hards",
                "Harry Arnold and His Swedish Radio Studio Orchestra",
                "Harvey Mason, Sr.",
                "Heart & Feather",
                "Henry Mancini And His Orchestra",
                "Hety And Zambo",
                "Hi, Brits",
                "Hillary Scott And The Scott Family",
                "Honey and Salt",
                "Hootie And The Blowfish",
                "Hoots and Hellmouth",
                "Horse + Donkey",
                "Huey Lewis & The News",
                "Ice & Cream",
                "Ike & Tina Turner",
                "Invincible & Finale",
                "I'm a Lion, I'm a Wolf",
                "Iron & Wine",
                "Jack & Jones",
                "Jack and the Ripper",
                "The Jack West & Lalo Vibe",
                "Jacques and the Shakey Boys",
                "Jackie's Boy",
                "James Moody And His Swedish Crowns",
                "Jam & Spoon",
                "JC & Co.",
                "The Jesus And Mary Chain",
                "Jim and Sam",
                "Jon And Vangelis",
                "John Fred & His Playboy Band",
                "John Makin & Friends",
                "Johnny Kidd And The Pirates",
                "Johnny & The Hurricanes",
                "John Schooley and his One Man Band",
                "Jon And Vangelis",
                "Judah & the Lion",
                "Juliette and the Licks",
                "Katrina and the Waves",
                "Kawai, Mal Duo",
                "KC & The Sunshine Band",
                "Kevin Devine & the Goddamn Band",
                "Kid Congo & The Pink Monkey Birds",
                "King Gizzard & The Lizard Wizard",
                "Kingman and Jonah",
                "Kitty, Daisy & Lewis",
                "Kool and the Gang",
                "Kowton's Feeling Fragile",
                "Kruder & Dorfmeister",
                "Kunstner 5",
                "Kurt Stifle and the Swing Shift",
                "Lady Dottie and the Diamonds",
                "The Last Artful, Dodgr",
                "The Lighthouse and the Whaler",
                "Lights & Motion",
                "Link Wray & His Ray men",
                "Liz Cooper and the Stampede",
                "Lizzie and The Makers",
                "Lloyd Marcus & Friends",
                "Lorelle Meets The Obsolete",
                "Love And Rockets",
                "Love X Stereo",
                "Luca Rovini & Companeros",
                "Lyons & Co.",
                "Magic & Naked",
                "The Main Ingredient",
                "The Main Squeez",
                "The Main Street Gospel",
                "The Mamas & The Papas",
                "Maps & Atlases",
                "Margot & The Nuclear So and So",
                "Mark Pickerel & His Praying Hands",
                "Marlon and the Shakes",
                "Maroon 5",
                "Martha and the Muffins",
                "M & B",
                "Medi And The Medicine Show",
                "ME & MARIE",
                "Me & My",
                "Me And My Brother",
                "Me and That Man",
                "Me And You",
                "Mike And The Mechanics",
                "Mike and the Moonpies",
                "Mike Nyoni and Born Free",
                "Mikey And The Drags",
                "Milk & Bone",
                "Missi & Mister Baker",
                "Mistress Stephanie And Her Melodic Cat",
                "Mixtapes & Cellmates",
                "The M&M",
                "Mono & Nikitaman",
                "Mono & World's End Girlfriend",
                "The Moth & The Flame",
                "Mouth & MacNeal",
                "Mozes and the Firstborn",
                "The MPS Rhythm Combination & Brass",
                "Mr. & Mrs. Smith",
                "Muck and the Mires",
                "Mum and DAD",
                "Mumford & Sons",
                "Murder By Death",
                "Myka 9",
                "MY RED + BLUE",
                "The Naked And Famous",
                "Nakia & His Southern Cousins",
                "Nalin & Kane",
                "Nick Cave And The Bad Seeds",
                "The Nirvana Sitar & String Group",
                "Noah and the Whale",
                "Nostalgia 77 Octet",
                "nothing,nowhere.",
                "Now, Now",
                "Obscured By Echoes",
                "Of Monsters & Men",
                "Of The Wand And The Moon",
                "Oh,Beast!",
                "Oh, Rose",
                "Orchid and Hound",
                "Or, the Whale",
                "O + S",
                "Oxford & Co.",
                "Paddy O'Connor & Friends",
                "Part 1",
                "Parts & Labor",
                "Part-Time Friends",
                "Part Time",
                "Peaches & Herb",
                "Penny and Sparrow",
                "Pete and the Pirates",
                "Peter and the Wolf",
                "Peter, Bjorn and John",
                "Pets With Pets",
                "Phil and the Osophers",
                "Phlash & Friends",
                "Plants and Animals",
                "Polly Mackey & the Pleasure Principle",
                "Prefuse 73",
                "Pretty and Nice",
                "Prince & The Revolution",
                "Prinz Grizzley and his Beargaroos",
                "Project Jenny, Project Jan",
                "Quantic and his Combo Barbaro",
                "Quien es, Boom!",
                "Radio B.",
                "Rae & Christian",
                "Rafter and Friends",
                "Ravens & Chimes",
                "Rene Riche And Her Cosmic Band",
                "Reverend Glasseye and His Wooden Legs",
                "Robots & Balloons",
                "Rosemary & Garlic",
                "Rosie And The Goldbug",
                "Royal Family and The Poor",
                "Russ Garcia & His Orchestra",
                "Sad Lovers & Giants",
                "Seals and Crofts",
                "Secos And Molhados",
                "Semi Sheheen and his Utica Ensemble",
                "Sex With Strangers",
                "Sgt Dunbar and the Hobo Banned",
                "Shapes and Sizes",
                "She & Him",
                "Shy, Low",
                "Shovels & Rope",
                "Shot x Shot",
                "Simon & Garfunkel",
                "Siouxsie & the Banshees",
                "Sister Sparrow & The Dirty Birds",
                "The Skatalites And Disco Height",
                "Sly & The Family Stone",
                "Snake & Jet's Amazing Bullit Band",
                "Soiled Mattress & The Springs",
                "Some By Sea",
                "Something With Numbers",
                "Spottiswoode & His Enemies",
                "Stars of Track and Field",
                "Stella By Starlight",
                "Stew Cutler & Friends",
                "Stones & Bones",
                "Stoni Taylor & Miles Of Stones",
                "Sugar Dirt and Sand",
                "Sugar & Gold",
                "Sun And The Wolf",
                "Super Onze, The Takamba champions of the Niger bend.",
                "Susan Gibson and The Moving Parts",
                "Swimming With Bears",
                "Swimming With Dolphins",
                "Sy Oliver & His Orchestra",
                "Tacks, the Boy Disaster",
                "Take 6",
                "Take Over And Destroy",
                "Talisman & Hudson",
                "Tank and the Bangas",
                "Thieves by the Code",
                "The Thing With Five Eyes",
                "Thom And The Wolves",
                "Thor & Friends",
                "Tim & Adam",
                "To Live And Die In LA",
                "Toots And The Maytals",
                "Touch & Go",
                "T'nah Apex",
                "Two Hoots and a Holler",
                "Tyler, The Creator",
                "Up, Bustle And Out",
                "Us, Today",
                "Van and Schenck",
                "Wildbirds & Peacedrums",
                "William and Versey Smith",
                "The Wind & The Wave",
                "Winter & Triptides",
                "White Shoes & The Couples Company",
                "The Whistles & The Bells",
                "Wine and Revolution",
                "Women & Children",
                "Woody Herman And His Orchestra",
                "Wood & Wire",
                "The World Is A Beautiful Place & I Am No Longer Afraid To Die",
                "Wreck and Reference",
                "Years & Years",
                "Young & Sick",
                "You, Me, And Everyone We Know",
                "Yourself and The Air",
                "Zager & Evans",
                "Zeal and Ardor",
                "Zolof the Rock & Roll Destroyer",
            };
        }

        public static class Spliters
        {
            public static char WordsSimpleSplitter = ' ';
            public static string FeaturedArtistsInTheTitle = @"\([^)]*\)";
        }
        public static class Transforms
        {
            public static Dictionary<string, string> ArtistNamesMutation = new Dictionary<string, string>
            {
                {"ac; dc", "ac/dc"},
                {"ac;dc", "ac/dc"},
                {"dido armstrong", "dido"},
                {"elo", "electric light orchestra" },
                {"lorenzo \"jovanotti\" cherubini", "jovanotti"},
                {"林默", "missmo"},
                {"mихайло xай", "mihaylo hai"},
                {"桜庭統", "motoi sakuraba"},
                {"Ω▽", "ohmslice" },
                {"rollo", "rollo armstrong"},
                {"つじあやの", "tsuji ayano"},
            };

            public static Dictionary<string, string> LatinAlphabetTransformations = new Dictionary<string, string>
            {
                {"á","a" },
                {"à","a" },
                {"ä", "a" },
                {"Å","a" },
                {"â","a" },
                {"ã","a" },
                {"å","a" },
                {"æ","a" },
                {"ß","s" },
                {"č","c" },
                {"ć", "c" },
                {"Ç", "c" },
                {"é", "e" },
                {"è","e" },
                {"ë", "e" },
                {"Ê", "e" },
                {"ė","e" },
                {"ğ", "g" },
                {"í","i" },
                {"ï","i" },
                {"Î","i" },
                {"ñ","n" },
                {"ń","n" },
                {"ó","o" },
                {"Ö","o" },
                {"ø","o" },
                {"ô","o" },
                {"ō","o" },
                {"ř","r" },
                {"š","s" },
                { "ș","s"},
                {"$","s" },
                {"ü","u" },
                {"ú","u" },
                {"ū","u" },
                {"ý","y" },
                {"Λ","&" },
                {"�","i" },
            };


        }

        public static class Rules
        {
            //99% chance for being a band for artist that start with "The ", "El ", "My " or "New "
            public static string[] BandStartWords = new[] 
                { "el", "my", "new", "the",  };

            //High chance for artist contain some words to be a band 
            public static string[] BandWords = new[]
            {
                "a",
                "alliance",
                "all",
                "an",
                "association",
                "at",
                "band",
                "banda",
                "boys",
                "bros",
                "brothers",
                "chamber",
                "choir",
                "chorale",
                "city",
                "club",
                "collective",
                "committee",
                "der",
                "duo",
                "ensemble",
                "et",
                "etc.",
                "experience",
                "family",
                "for",
                "foundation",
                "friends",
                "gang",
                "girls",
                "grand",
                "group",
                "grupo",
                "in",
                "kids",
                "kolektiv",
                "las",
                "les",
                "los",
                "men",
                "n'",
                "of",
                "on",
                "or",
                "orchestra",
                "orchestre",
                "orkestar",
                "philarmonic",
                "quartet",
                "quintet",
                "quintetto",
                "royal",
                "sisters",
                "society",
                "squad",
                "symphony",
                "to",
                "trio",
                "twins",
                "und",
                "we",
                "y",
                
            };

            public static int MaxWordsPerArtist = 4;

            public static string[] exceptionRules = new[]
            {
                "2pac",
                "alvin pleasant delaney carter",
                "anna of the north",
                "antonio carles marques pinto",
                "aram mp3",
                "b l a c k i e", 
                "boydston john d. iii",
                "jennifer evans van der harten",
                "john 5",
                "kh of moscrill",
                @"luther ""guitar junior"" johnson",
                "maria isabel garcia asensio",
                "michael l. williams ii",
                "michiel van der kuy",
                "niels henning orsted pedersen",
                "the reverend peyton",
                "armistead burwell smith iv",
            };
        }
    }
}